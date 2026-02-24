from __future__ import annotations

import json
import queue
import socket
import threading
from typing import Any


class NetworkClient:
    def __init__(self) -> None:
        self._socket: socket.socket | None = None
        self._socket_lock = threading.Lock()
        self._reader_thread: threading.Thread | None = None
        self._stop_event = threading.Event()
        self._messages: queue.Queue[dict[str, Any]] = queue.Queue()
        self.connected = False

    def connect(self, host: str, port: int, timeout_sec: float = 6.0) -> None:
        self.close()

        sock = socket.create_connection((host, port), timeout=timeout_sec)
        sock.settimeout(None)
        self._socket = sock
        self.connected = True
        self._stop_event.clear()

        self._reader_thread = threading.Thread(target=self._reader_loop, daemon=True)
        self._reader_thread.start()

    def _reader_loop(self) -> None:
        assert self._socket is not None
        try:
            buffer = b""
            while not self._stop_event.is_set():
                chunk = self._socket.recv(4096)
                if not chunk:
                    break
                buffer += chunk
                while b"\n" in buffer:
                    line, buffer = buffer.split(b"\n", 1)
                    if not line:
                        continue
                    try:
                        message = json.loads(line.decode("utf-8"))
                        self._messages.put(message)
                    except json.JSONDecodeError:
                        self._messages.put({"type": "error", "message": "Invalid server message"})
        except OSError:
            pass
        finally:
            self.connected = False
            self._messages.put({"type": "disconnect", "message": "Connection closed"})

    def send(self, message_type: str, **payload: Any) -> bool:
        if not self.connected or self._socket is None:
            return False

        data = {"type": message_type, **payload}
        wire = (json.dumps(data, ensure_ascii=False) + "\n").encode("utf-8")

        try:
            with self._socket_lock:
                self._socket.sendall(wire)
            return True
        except OSError:
            self.connected = False
            return False

    def poll(self, limit: int = 100) -> list[dict[str, Any]]:
        messages: list[dict[str, Any]] = []
        for _ in range(limit):
            try:
                messages.append(self._messages.get_nowait())
            except queue.Empty:
                break
        return messages

    def close(self) -> None:
        self._stop_event.set()
        if self._socket is not None:
            try:
                self._socket.shutdown(socket.SHUT_RDWR)
            except OSError:
                pass
            try:
                self._socket.close()
            except OSError:
                pass
        self._socket = None
        self.connected = False
