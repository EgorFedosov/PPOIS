"""JSON persistence for CookingProcess snapshots."""

from __future__ import annotations

import json
from pathlib import Path
from typing import Any


class JsonStorage:
    """Persist and restore application state from a JSON file."""

    def __init__(self, filepath: str | Path) -> None:
        self.filepath = Path(filepath)

    def load(self) -> dict[str, Any] | None:
        """Load snapshot dict from disk if present and valid."""

        if not self.filepath.exists():
            return None

        try:
            raw = self.filepath.read_text(encoding="utf-8")
            data = json.loads(raw)
            if isinstance(data, dict):
                return data
        except (OSError, json.JSONDecodeError):
            return None

        return None

    def save(self, snapshot: dict[str, Any]) -> None:
        """Save snapshot dict to disk."""

        self.filepath.parent.mkdir(parents=True, exist_ok=True)
        payload = json.dumps(snapshot, ensure_ascii=False, indent=2)
        self.filepath.write_text(payload, encoding="utf-8")
