"""CLI-меню для управления процессом готовки."""

from __future__ import annotations

from src.exceptions import CookingError
from src.models.cooking_process import CookingProcess
from src.storage.json_storage import JsonStorage


class Menu:
    """Простое консольное меню с бесконечным циклом."""

    def __init__(self, process: CookingProcess, storage: JsonStorage) -> None:
        self.process = process
        self.storage = storage

    def run(self) -> None:
        """Запустить CLI-цикл до выхода пользователя."""

        while True:
            print("\n=== Модель готовки яичницы ===")
            print(f"Состояние: {self.process.state.value}")
            print(f"Плита: {'вкл' if self.process.stove.is_on else 'выкл'}")
            print(f"Сковорода: {'горячая' if self.process.pan.is_hot else 'холодная'}")
            print(f"Яйцо: {self.process.egg.state.value}")
            print("1. Разогреть сковороду")
            print("2. Разбить яйца")
            print("3. Добавить масло")
            print("4. Добавить приправы")
            print("5. Обжарить яйца")
            print("6. Перемешать и подать")
            print("7. Выход с сохранением")

            choice = input("Выберите команду: ").strip()
            if choice == "7":
                self.storage.save(self.process.snapshot())
                print("Состояние сохранено. Выход.")
                break

            try:
                self._execute(choice)
                self.storage.save(self.process.snapshot())
            except CookingError as error:
                print(f"Ошибка: {error}")
            except ValueError as error:
                print(f"Ошибка: {error}")

    def _execute(self, choice: str) -> None:
        if choice == "1":
            print(self.process.heat_pan())
        elif choice == "2":
            print(self.process.break_eggs())
        elif choice == "3":
            print(self.process.add_oil())
        elif choice == "4":
            print(self.process.add_spices())
        elif choice == "5":
            print(self.process.fry_eggs())
        elif choice == "6":
            print(self.process.mix_and_serve())
        else:
            print("Неизвестная команда.")
