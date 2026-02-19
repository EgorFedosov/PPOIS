"""Application entry point."""

from pathlib import Path

from src.models.cooking_process import CookingProcess
from src.models.entities import Egg, Oil, Pan, Spatula, Spices, Stove
from src.storage.json_storage import JsonStorage
from src.ui.menu import Menu


def create_default_process() -> CookingProcess:
    return CookingProcess(
        stove=Stove(),
        pan=Pan(),
        spatula=Spatula(),
        egg=Egg(),
        oil=Oil(),
        spices=Spices(),
    )


def main() -> None:
    storage = JsonStorage(Path("data") / "state.json")
    snapshot = storage.load()
    process = CookingProcess.from_snapshot(snapshot) if snapshot else create_default_process()
    menu = Menu(process, storage)
    menu.run()


if __name__ == "__main__":
    main()
