from __future__ import annotations

import json
import socket
import socketserver
import threading
import time
from dataclasses import dataclass, field
from pathlib import Path
from typing import Any

from src.core.minesweeper import MinesweeperGame

ROOT_DIR = Path(__file__).resolve().parents[2]


def load_json(path: Path) -> dict[str, Any]:
    return json.loads(path.read_text(encoding="utf-8"))


@dataclass
class ClientSession:
    client_id: int
    conn: socket.socket
    name: str
    send_lock: threading.Lock = field(default_factory=threading.Lock, repr=False)

    def send(self, payload: dict[str, Any]) -> bool:
        wire = (json.dumps(payload, ensure_ascii=False) + "\n").encode("utf-8")
        try:
            with self.send_lock:
                self.conn.sendall(wire)
            return True
        except OSError:
            return False

    def close(self) -> None:
        try:
            self.conn.shutdown(socket.SHUT_RDWR)
        except OSError:
            pass
        try:
            self.conn.close()
        except OSError:
            pass


class GameState:
    def __init__(self, levels: dict[str, Any], default_level: str, lobby_size: int) -> None:
        self.levels = levels
        self.default_level = default_level
        self.level_name = default_level
        self.lobby_size = lobby_size

        self.lock = threading.Lock()
        self.next_client_id = 1
        self.clients: dict[int, ClientSession] = {}

        self.game = self._new_game(default_level)
        self.last_elapsed_broadcast = -1

    def _new_game(self, level_name: str) -> MinesweeperGame:
        level = self.levels[level_name]
        return MinesweeperGame(
            width=int(level["width"]),
            height=int(level["height"]),
            mine_count=int(level["mines"]),
            time_limit_sec=int(level["time_limit_sec"]),
        )

    def register_client(self, conn: socket.socket) -> int | None:
        with self.lock:
            if len(self.clients) >= self.lobby_size:
                wire = json.dumps({"type": "error", "message": "Lobby is full"}, ensure_ascii=False) + "\n"
                conn.sendall(wire.encode("utf-8"))
                return None

            client_id = self.next_client_id
            self.next_client_id += 1
            session = ClientSession(client_id=client_id, conn=conn, name=f"Player {client_id}")
            self.clients[client_id] = session

        session.send({"type": "welcome", "player_id": client_id})
        self.broadcast_lobby()
        return client_id

    def unregister_client(self, client_id: int) -> None:
        session: ClientSession | None = None
        should_reset = False

        with self.lock:
            session = self.clients.pop(client_id, None)
            should_reset = len(self.clients) < self.lobby_size
            if should_reset:
                self.game = self._new_game(self.level_name)
                self.last_elapsed_broadcast = -1

        if session is not None:
            session.close()

        self.broadcast_lobby()
        if should_reset:
            self.broadcast_state()

    def handle_message(self, client_id: int, message: dict[str, Any]) -> None:
        msg_type = str(message.get("type", "")).lower()

        if msg_type == "join":
            name = self._sanitize_name(str(message.get("name", "")))
            with self.lock:
                if client_id in self.clients:
                    self.clients[client_id].name = name
            self.broadcast_lobby()
            self.auto_start_if_ready()
            return

        if msg_type == "new_game":
            level_name = str(message.get("level", self.default_level)).lower()
            self.start_new_game(level_name)
            return

        if msg_type in {"reveal", "flag"}:
            x = int(message.get("x", -1))
            y = int(message.get("y", -1))
            self.apply_action(msg_type, x, y)
            return

        if msg_type == "ping":
            self.send_to(client_id, {"type": "pong", "ts": time.time()})
            return

        self.send_to(client_id, {"type": "error", "message": f"Unknown message type: {msg_type}"})

    def _sanitize_name(self, value: str) -> str:
        cleaned = "".join(ch for ch in value[:16] if ch.isalnum() or ch in " _-").strip()
        return cleaned or "Player"

    def _payload_locked(self) -> dict[str, Any]:
        payload = self.game.public_state()
        payload["level"] = self.level_name
        payload["players"] = [{"id": cid, "name": s.name} for cid, s in sorted(self.clients.items())]
        return payload

    def send_to(self, client_id: int, payload: dict[str, Any]) -> None:
        with self.lock:
            session = self.clients.get(client_id)
        if session is not None:
            session.send(payload)

    def broadcast(self, payload: dict[str, Any]) -> None:
        with self.lock:
            sessions = list(self.clients.values())
        for session in sessions:
            session.send(payload)

    def broadcast_lobby(self) -> None:
        with self.lock:
            players = [{"id": cid, "name": s.name} for cid, s in sorted(self.clients.items())]
            payload = {
                "type": "lobby",
                "players": players,
                "needed": self.lobby_size,
                "can_start": len(players) >= self.lobby_size,
                "level": self.level_name,
            }
        self.broadcast(payload)

    def broadcast_state(self) -> None:
        with self.lock:
            payload = {"type": "state", "payload": self._payload_locked()}
        self.broadcast(payload)

    def start_new_game(self, requested_level: str) -> None:
        with self.lock:
            if requested_level in self.levels:
                self.level_name = requested_level

            enough_players = len(self.clients) >= self.lobby_size
            if enough_players:
                self.game = self._new_game(self.level_name)
                self.last_elapsed_broadcast = -1
                state_payload = {"type": "state", "payload": self._payload_locked()}
                info_payload = {"type": "info", "message": f"New round started: {self.level_name}"}
            else:
                state_payload = None
                info_payload = {"type": "info", "message": f"Need {self.lobby_size} players to start"}

        self.broadcast(info_payload)
        if state_payload is not None:
            self.broadcast(state_payload)

    def apply_action(self, action: str, x: int, y: int) -> None:
        with self.lock:
            if len(self.clients) < self.lobby_size:
                return

            changed = False
            if action == "reveal":
                changed = self.game.reveal(x, y)
            elif action == "flag":
                changed = self.game.toggle_flag(x, y)

            if self.game.tick():
                changed = True

            payload = {"type": "state", "payload": self._payload_locked()} if changed else None

        if payload is not None:
            self.broadcast(payload)

    def auto_start_if_ready(self) -> None:
        with self.lock:
            should_start = len(self.clients) >= self.lobby_size and self.game.status() == "waiting"
        if should_start:
            self.start_new_game(self.level_name)

    def ticker_loop(self, stop_event: threading.Event) -> None:
        while not stop_event.wait(0.2):
            with self.lock:
                status = self.game.status()
                changed = self.game.tick()
                elapsed = self.game.elapsed_sec()

                should_broadcast = changed
                if status == "running" and elapsed != self.last_elapsed_broadcast:
                    should_broadcast = True

                payload = None
                if should_broadcast:
                    self.last_elapsed_broadcast = elapsed
                    payload = {"type": "state", "payload": self._payload_locked()}

            if payload is not None:
                self.broadcast(payload)


class MinesweeperRequestHandler(socketserver.StreamRequestHandler):
    def handle(self) -> None:
        state: GameState = self.server.state  # type: ignore[attr-defined]
        client_id = state.register_client(self.request)
        if client_id is None:
            return

        try:
            for raw_line in self.rfile:
                if not raw_line:
                    break
                line = raw_line.decode("utf-8").strip()
                if not line:
                    continue
                try:
                    message = json.loads(line)
                except json.JSONDecodeError:
                    state.send_to(client_id, {"type": "error", "message": "Invalid JSON"})
                    continue
                state.handle_message(client_id, message)
        finally:
            state.unregister_client(client_id)


class ThreadedTCPServer(socketserver.ThreadingTCPServer):
    allow_reuse_address = True
    daemon_threads = True

    def __init__(self, server_address: tuple[str, int], handler_cls: type[socketserver.BaseRequestHandler], state: GameState) -> None:
        super().__init__(server_address, handler_cls)
        self.state = state


def main() -> None:
    network_cfg = load_json(ROOT_DIR / "config" / "network.json")
    levels_cfg = load_json(ROOT_DIR / "config" / "levels.json")

    host = str(network_cfg.get("host", "127.0.0.1"))
    port = int(network_cfg.get("port", 5050))
    lobby_size = int(network_cfg.get("lobby_size", 2))

    default_level = str(levels_cfg.get("default_level", "medium"))
    levels = dict(levels_cfg.get("levels", {}))
    if default_level not in levels:
        default_level = next(iter(levels))

    state = GameState(levels=levels, default_level=default_level, lobby_size=lobby_size)
    stop_event = threading.Event()

    ticker_thread = threading.Thread(target=state.ticker_loop, args=(stop_event,), daemon=True)
    ticker_thread.start()

    with ThreadedTCPServer((host, port), MinesweeperRequestHandler, state) as server:
        print(f"Server started on {host}:{port}")
        try:
            server.serve_forever()
        except KeyboardInterrupt:
            print("Server interrupted")
        finally:
            stop_event.set()
            server.shutdown()


if __name__ == "__main__":
    main()
