"""Application service that encapsulates process state and persistence."""

from __future__ import annotations

from pathlib import Path
from threading import Lock
from typing import Callable

try:
    from L4.shared import create_default_process
    from L4.shared.models.cooking_process import CookingProcess
    from L4.shared.storage.json_storage import JsonStorage
except ModuleNotFoundError:
    from shared import create_default_process
    from shared.models.cooking_process import CookingProcess
    from shared.storage.json_storage import JsonStorage

ActionMethod = Callable[[], str]


class UnknownActionError(ValueError):
    """Raised when API receives unsupported action name."""


class CookingService:
    """Stateful service used by both API handlers and UI."""

    def __init__(self, state_file: Path) -> None:
        self._storage = JsonStorage(state_file)
        snapshot = self._storage.load()
        self._process = (
            CookingProcess.from_snapshot(snapshot)
            if snapshot
            else create_default_process()
        )
        self._lock = Lock()

    def get_state(self) -> dict[str, object]:
        with self._lock:
            return self._process.snapshot()

    def execute(self, action: str) -> tuple[str, dict[str, object]]:
        with self._lock:
            action_map: dict[str, ActionMethod] = {
                "heat_pan": self._process.heat_pan,
                "break_eggs": self._process.break_eggs,
                "add_oil": self._process.add_oil,
                "add_spices": self._process.add_spices,
                "fry_eggs": self._process.fry_eggs,
                "mix_and_serve": self._process.mix_and_serve,
            }

            operation = action_map.get(action)
            if operation is None:
                raise UnknownActionError(f"Unknown action: {action}")

            message = operation()
            snapshot = self._process.snapshot()
            self._storage.save(snapshot)
            return message, snapshot

    def reset(self) -> dict[str, object]:
        """Drop persisted state and create a fresh process."""

        with self._lock:
            self._process = create_default_process()
            try:
                self._storage.filepath.unlink(missing_ok=True)
            except OSError:
                pass

            snapshot = self._process.snapshot()
            self._storage.save(snapshot)
            return snapshot
