"""CLI entry point for Lab 4."""

from pathlib import Path

try:
    from L4.cli.menu import Menu
    from L4.shared import create_default_process
    from L4.shared.models.cooking_process import CookingProcess
    from L4.shared.storage.json_storage import JsonStorage
except ModuleNotFoundError:
    from cli.menu import Menu
    from shared import create_default_process
    from shared.models.cooking_process import CookingProcess
    from shared.storage.json_storage import JsonStorage


def main() -> None:
    base_dir = Path(__file__).resolve().parent
    storage = JsonStorage(base_dir / "data" / "state.json")
    snapshot = storage.load()
    process = CookingProcess.from_snapshot(snapshot) if snapshot else create_default_process()
    menu = Menu(process, storage)
    menu.run()


if __name__ == "__main__":
    main()
