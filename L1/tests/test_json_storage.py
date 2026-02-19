from pathlib import Path

from src.storage.json_storage import JsonStorage


def test_load_returns_none_when_file_missing() -> None:
    filepath = Path("data") / "test_missing.json"
    storage = JsonStorage(filepath)
    assert storage.load() is None


def test_save_and_load_roundtrip() -> None:
    filepath = Path("data") / "test_state.json"
    storage = JsonStorage(filepath)
    snapshot = {"state": "idle", "stove": {"is_on": False}}

    storage.save(snapshot)
    loaded = storage.load()
    assert loaded == snapshot
