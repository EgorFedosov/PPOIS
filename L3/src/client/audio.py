from __future__ import annotations

from pathlib import Path

import pygame


AUDIO_FILES = {
    "click": "click.wav",
    "flag": "flag.wav",
    "boom": "boom.wav",
    "win": "win.wav",
    "music": "music.wav",
}


class AudioManager:
    def __init__(self, audio_dir: Path, music_volume: float, sfx_volume: float) -> None:
        self.paths = {key: audio_dir / filename for key, filename in AUDIO_FILES.items()}
        self.sounds: dict[str, pygame.mixer.Sound] = {}
        self.music_volume = float(music_volume)

        for key in ("click", "flag", "boom", "win"):
            path = self.paths[key]
            if not path.exists():
                continue
            try:
                sound = pygame.mixer.Sound(str(path))
                sound.set_volume(float(sfx_volume))
                self.sounds[key] = sound
            except pygame.error:
                continue

    def start_music(self) -> None:
        music_path = self.paths["music"]
        if not music_path.exists():
            return
        try:
            pygame.mixer.music.load(str(music_path))
            pygame.mixer.music.set_volume(self.music_volume)
            pygame.mixer.music.play(-1)
        except pygame.error:
            pass

    def play(self, name: str) -> None:
        sound = self.sounds.get(name)
        if sound is not None:
            sound.play()
