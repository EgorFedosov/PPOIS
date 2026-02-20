# Модель готовки яичницы (CLI)

Консольное приложение, которое моделирует приготовление яичницы как конечный автомат.

## Что реализовано
- Предметные сущности: `Stove`, `Pan`, `Spatula`, `Egg`, `Oil`, `Spices`.
- Контроллер процесса: `CookingProcess`.
- Состояния процесса: `IDLE`, `HEATING`, `COOKING`, `SEASONING`, `FINISHING`, `SERVED`.
- Операции сценария:
  - `heat_pan()`
  - `break_eggs()`
  - `add_oil()`
  - `add_spices()`
  - `fry_eggs()`
  - `mix_and_serve()`
- Сохранение/восстановление состояния:
  - `snapshot()`
  - `from_snapshot(...)`
  - `JsonStorage` (`data/state.json`)
- Пользовательские исключения:
  - `CookingError`
  - `EggError`
  - `InvalidStateTransitionError`

## Запуск
```bash
python main.py
```

## Тесты
```bash
pytest -q
```

## Структура проекта
- `main.py` - точка входа, создание процесса и запуск меню.
- `src/models/entities.py` - доменные сущности и `EggState`.
- `src/models/cooking_process.py` - конечный автомат (`CookingProcess`, `CookingState`).
- `src/storage/json_storage.py` - JSON-хранилище состояния.
- `src/ui/menu.py` - CLI-меню и автосохранение.
- `src/exceptions.py` - доменные исключения.
- `tests/test_cooking_process.py` - unit-тесты бизнес-логики.
- `tests/test_json_storage.py` - unit-тесты хранения.
- `docs/system.md` - описание системы.
- `docs/uml/class_diagram.puml` - диаграмма классов (PlantUML).
- `docs/uml/state_diagram.puml` - диаграмма состояний (PlantUML).