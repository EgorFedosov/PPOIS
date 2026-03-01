# ЛР4: Веб-интерфейс для модели готовки

Клиент-серверное приложение, построенное на результате ЛР1.

## Технологии

- Серверная часть: FastAPI
- Клиентская часть: HTML + Bootstrap 5 + JavaScript (`fetch`)
- Хранение состояния: JSON-файл (`data/state.json`)
- Взаимодействие: HTTP REST API

## Структура проекта

- `shared/` - общий код модели и хранения для CLI и веб-интерфейса.
- `cli/`, `main_cli.py` - консольный интерфейс.
- `app/` - сервер FastAPI.
- `web/index.html` - веб-интерфейс.

## HTTP API

- `GET /api/state` - получить текущее состояние модели.
- `POST /api/actions/heat_pan` - разогреть сковороду.
- `POST /api/actions/break_eggs` - разбить яйца.
- `POST /api/actions/add_oil` - добавить масло.
- `POST /api/actions/add_spices` - добавить приправы.
- `POST /api/actions/fry_eggs` - обжарить яйца.
- `POST /api/actions/mix_and_serve` - перемешать и подать.
- `POST /api/reset` - сбросить состояние и начать заново.

## Запуск

Установить зависимости (из каталога `L4`):

```bash
python -m pip install -r requirements.txt
```

Запуск:

```bash
python -m uvicorn app.main:app --reload
```

Адрес в браузере:

`http://127.0.0.1:8000`

Запуск консольной версии:

```bash
python -m main_cli
```