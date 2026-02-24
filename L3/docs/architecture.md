# Архитектура

## Слои

- `src/core/minesweeper.py`
  - Чистая логика сапера: открытие клеток, флаги, таймер, победа/поражение, счет.

- `src/client/client_app.py`
  - Точка сборки desktop-клиента.
  - Инициализация окна, конфигов, сервисов и общего состояния.

- `src/client/ui_mixins/common.py`
  - Общие UI-утилиты: масштаб, пересоздание UIManager, resize окна, общие хелперы.

- `src/client/ui_mixins/screens.py`
  - Сборка экранов `menu/help/records/lobby` и диалога нового рекорда (`pygame_gui`).

- `src/client/ui_mixins/gameplay.py`
  - Игровой flow: запуск solo/online, обработка сетевых сообщений, события ввода, главный loop.

- `src/client/ui_mixins/render.py`
  - Рендеринг игрового слоя на `pygame`: фон, карточки, поле, анимации, header.

- `src/network/client.py`
  - Сетевой клиент (JSON over TCP, отдельный поток чтения сообщений).

- `src/server/main.py`
  - Онлайн-сервер на `socketserver.ThreadingTCPServer`.
  - Хранит авторитетное состояние раунда и рассылает его двум игрокам.

- `src/client/audio.py`
  - Проигрывание статических аудио-файлов из `assets/audio`.

- `src/client/records.py`
  - Работа с таблицей рекордов в `data/records.json`.

## Режимы игры

- `solo`
  - Локальная игра без сервера.
  - Клиент напрямую использует `MinesweeperGame`.

- `online`
  - Два клиента подключаются к серверу.
  - Действия (`reveal`, `flag`, `new_game`) идут на сервер.
  - Сервер рассылает общее состояние всем участникам.

## Протокол

Формат: JSON-сообщения, разделенные `\n`.

Клиент → сервер:
- `join`
- `new_game`
- `reveal`
- `flag`
- `ping`

Сервер → клиент:
- `welcome`
- `lobby`
- `state`
- `info`
- `error`
- `pong`

## Конфигурация

Все ключевые параметры хранятся во внешних JSON:
- уровни и таймер (`config/levels.json`)
- UI/экран/звук (`config/app.json`)
- сеть (`config/network.json`)
- тексты справки (`config/help.json`)
- рекорды (`data/records.json`)
