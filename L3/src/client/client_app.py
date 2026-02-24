from __future__ import annotations

import getpass
import json
import os
from pathlib import Path
from typing import Any

import pygame
import pygame_gui
from pygame_gui.elements import UIButton, UILabel, UITextEntryLine, UIWindow

from src.client.audio import AudioManager
from src.client.records import load_records
from src.client.ui_mixins import UICommonMixin, UIGameplayMixin, UIRenderMixin, UIScreensMixin
from src.core.minesweeper import MinesweeperGame
from src.network.client import NetworkClient

ROOT_DIR = Path(__file__).resolve().parents[2]


def load_json(path: Path) -> dict[str, Any]:
    return json.loads(path.read_text(encoding="utf-8"))


class MinesweeperClientApp(UICommonMixin, UIScreensMixin, UIGameplayMixin, UIRenderMixin):
    def __init__(self) -> None:
        self.app_cfg = load_json(ROOT_DIR / "config" / "app.json")
        self.levels_cfg = load_json(ROOT_DIR / "config" / "levels.json")
        self.network_cfg = load_json(ROOT_DIR / "config" / "network.json")
        self.help_cfg = load_json(ROOT_DIR / "config" / "help.json")

        self.levels: dict[str, dict[str, Any]] = dict(self.levels_cfg["levels"])
        self.level_names = list(self.levels.keys())
        self.selected_level = str(self.levels_cfg.get("default_level", self.level_names[0]))
        if self.selected_level not in self.levels:
            self.selected_level = self.level_names[0]

        self.mode_options = ["solo", "online"]
        self.game_mode = "solo"

        self.records_path = ROOT_DIR / "data" / "records.json"
        self.records = load_records(self.records_path)

        if os.name == "nt":
            os.environ.setdefault("SDL_WINDOWS_DPI_AWARENESS", "permonitorv2")

        pygame.init()
        try:
            pygame.mixer.init(frequency=22050, size=-16, channels=1)
        except pygame.error:
            pass

        desktop_sizes = pygame.display.get_desktop_sizes()
        if desktop_sizes:
            self.desktop_width, self.desktop_height = desktop_sizes[0]
        else:
            display_info = pygame.display.Info()
            self.desktop_width, self.desktop_height = display_info.current_w, display_info.current_h

        cfg_width = int(self.app_cfg.get("width", self.desktop_width))
        cfg_height = int(self.app_cfg.get("height", self.desktop_height))
        if bool(self.app_cfg.get("use_desktop_resolution", True)):
            self.width = self.desktop_width
            self.height = self.desktop_height
        else:
            self.width = cfg_width
            self.height = cfg_height

        self.min_width = max(640, int(self.app_cfg.get("min_width", 960)))
        self.min_height = max(520, int(self.app_cfg.get("min_height", 700)))
        self.min_width = min(self.min_width, self.desktop_width)
        self.min_height = min(self.min_height, self.desktop_height)
        self.width = min(max(self.width, self.min_width), self.desktop_width)
        self.height = min(max(self.height, self.min_height), self.desktop_height)
        self.fps = int(self.app_cfg.get("fps", 60))
        self.base_cell_size = int(self.app_cfg.get("cell_size", 32))
        self.margin = int(self.app_cfg.get("margin", 16))
        self.ui_scale = float(self.app_cfg.get("ui_scale", 1.0))
        self.ui_scale_min = float(self.app_cfg.get("ui_scale_min", 0.8))
        self.ui_scale_max = float(self.app_cfg.get("ui_scale_max", 1.8))
        self.ui_scale_step = float(self.app_cfg.get("ui_scale_step", 0.1))
        self.ui_scale = max(self.ui_scale_min, min(self.ui_scale_max, self.ui_scale))

        self.window_flags = 0
        if bool(self.app_cfg.get("resizable_window", True)):
            self.window_flags |= pygame.RESIZABLE
        if bool(self.app_cfg.get("start_maximized", True)):
            self.window_flags |= pygame.WINDOWMAXIMIZED

        self.screen = pygame.display.set_mode((self.width, self.height), self.window_flags)
        self.width, self.height = self.screen.get_size()
        pygame.display.set_caption(str(self.app_cfg.get("title", "Minesweeper")))

        self.font_name = str(self.app_cfg.get("font_name", "arial"))
        self.base_font_size = int(self.app_cfg.get("font_size", 24))
        self.base_small_font_size = int(self.app_cfg.get("small_font_size", 18))
        self.font = pygame.font.SysFont(self.font_name, max(12, self._s(self.base_font_size)))
        self.small_font = pygame.font.SysFont(self.font_name, max(10, self._s(self.base_small_font_size)))
        self.number_fonts: dict[int, pygame.font.Font] = {}

        self.colors = {name: tuple(value) for name, value in self.app_cfg["colors"].items()}

        self.ui_theme_source_path = ROOT_DIR / str(self.app_cfg.get("ui_theme", "config/ui_theme.json"))
        self.ui_theme_runtime_path = ROOT_DIR / "data" / "_ui_theme_scaled.json"
        self.ui_theme_template: dict[str, Any] | None = None
        if self.ui_theme_source_path.exists():
            self.ui_theme_template = load_json(self.ui_theme_source_path)

        scaled_theme_path = self._build_scaled_theme_path()
        if scaled_theme_path is not None:
            self.ui = pygame_gui.UIManager((self.width, self.height), scaled_theme_path)
        else:
            self.ui = pygame_gui.UIManager((self.width, self.height))
        self.scene = "menu"
        self.scene_widgets: list[Any] = []
        self.actions: dict[UIButton, str] = {}

        self.audio = AudioManager(
            audio_dir=ROOT_DIR / "assets" / "audio",
            music_volume=float(self.app_cfg.get("music_volume", 0.35)),
            sfx_volume=float(self.app_cfg.get("sfx_volume", 0.7)),
        )
        self.audio.start_music()

        self.net = NetworkClient()
        base_name = str(self.network_cfg.get("default_player_name", "Player"))
        username = getpass.getuser() if os.name != "nt" else os.environ.get("USERNAME", "user")
        self.player_name = f"{base_name}-{username}"[:16]

        self.running = True
        self.status_line = ""
        self.status_timer = 0.0

        self.lobby_info: dict[str, Any] = {
            "players": [],
            "needed": int(self.network_cfg.get("lobby_size", 2)),
            "can_start": False,
        }

        self.board_state: dict[str, Any] | None = None
        self.local_game: MinesweeperGame | None = None
        self.local_last_elapsed = -1

        self.reveal_anim: dict[tuple[int, int], float] = {}
        self.death_anim_active = False
        self.death_anim_center: tuple[int, int] | None = None
        self.death_anim_time = 0.0

        self.record_window: UIWindow | None = None
        self.record_entry: UITextEntryLine | None = None
        self.pending_score: int | None = None

        self.lobby_status: UILabel | None = None
        self.lobby_p1: UILabel | None = None
        self.lobby_p2: UILabel | None = None

        self.background_surface = self._build_background_surface()
        self._build_menu()
