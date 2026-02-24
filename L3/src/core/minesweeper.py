from __future__ import annotations

import random
import time
from collections import deque
from dataclasses import dataclass, field
from typing import Optional

Coord = tuple[int, int]


@dataclass
class MinesweeperGame:
    width: int
    height: int
    mine_count: int
    time_limit_sec: int
    rng: random.Random = field(default_factory=random.Random)

    mines: set[Coord] = field(default_factory=set, init=False)
    revealed: set[Coord] = field(default_factory=set, init=False)
    flags: set[Coord] = field(default_factory=set, init=False)
    exploded: Optional[Coord] = field(default=None, init=False)

    started_at: Optional[float] = field(default=None, init=False)
    finished_at: Optional[float] = field(default=None, init=False)
    finished: bool = field(default=False, init=False)
    end_reason: Optional[str] = field(default=None, init=False)
    score: int = field(default=0, init=False)

    def __post_init__(self) -> None:
        total_cells = self.width * self.height
        if total_cells <= 0:
            raise ValueError("Board size must be positive")
        if not 1 <= self.mine_count < total_cells:
            raise ValueError("Mine count must be between 1 and board_size-1")
        if self.time_limit_sec <= 0:
            raise ValueError("Time limit must be positive")
        self.reset()

    def reset(self) -> None:
        self.mines.clear()
        self.revealed.clear()
        self.flags.clear()
        self.exploded = None
        self.started_at = None
        self.finished_at = None
        self.finished = False
        self.end_reason = None
        self.score = 0
        self._place_mines(exclude=None)

    def _place_mines(self, exclude: Optional[Coord]) -> None:
        cells = [(x, y) for y in range(self.height) for x in range(self.width)]
        if exclude is not None:
            cells = [cell for cell in cells if cell != exclude]
        self.mines = set(self.rng.sample(cells, self.mine_count))

    def _in_bounds(self, x: int, y: int) -> bool:
        return 0 <= x < self.width and 0 <= y < self.height

    def _neighbors(self, x: int, y: int) -> list[Coord]:
        result: list[Coord] = []
        for ny in range(y - 1, y + 2):
            for nx in range(x - 1, x + 2):
                if (nx, ny) == (x, y):
                    continue
                if self._in_bounds(nx, ny):
                    result.append((nx, ny))
        return result

    def _adjacent_mines(self, x: int, y: int) -> int:
        return sum((nx, ny) in self.mines for nx, ny in self._neighbors(x, y))

    def _start_timer_if_needed(self) -> None:
        if self.started_at is None:
            self.started_at = time.monotonic()

    def _relocate_mine(self, x: int, y: int) -> None:
        coord = (x, y)
        if coord not in self.mines:
            return
        self.mines.remove(coord)
        for ny in range(self.height):
            for nx in range(self.width):
                candidate = (nx, ny)
                if candidate != coord and candidate not in self.mines:
                    self.mines.add(candidate)
                    return

    def reveal(self, x: int, y: int) -> bool:
        if not self._in_bounds(x, y) or self.finished:
            return False

        coord = (x, y)
        if coord in self.flags or coord in self.revealed:
            return False

        first_reveal = self.started_at is None
        self._start_timer_if_needed()

        if first_reveal:
            self._relocate_mine(x, y)

        if coord in self.mines:
            self.revealed.add(coord)
            self.exploded = coord
            self._finish("lost")
            return True

        queue: deque[Coord] = deque([coord])
        while queue:
            cx, cy = queue.popleft()
            current = (cx, cy)
            if current in self.revealed or current in self.flags:
                continue

            self.revealed.add(current)
            if self._adjacent_mines(cx, cy) == 0:
                for nx, ny in self._neighbors(cx, cy):
                    neighbor = (nx, ny)
                    if neighbor in self.revealed or neighbor in self.flags or neighbor in self.mines:
                        continue
                    queue.append(neighbor)

        self._check_win()
        return True

    def toggle_flag(self, x: int, y: int) -> bool:
        if not self._in_bounds(x, y) or self.finished:
            return False

        coord = (x, y)
        if coord in self.revealed:
            return False

        if coord in self.flags:
            self.flags.remove(coord)
        else:
            self.flags.add(coord)
        return True

    def tick(self) -> bool:
        if self.finished or self.started_at is None:
            return False
        if self.elapsed_sec() >= self.time_limit_sec:
            self._finish("time")
            return True
        return False

    def _check_win(self) -> None:
        safe_cells = self.width * self.height - self.mine_count
        revealed_safe = len(self.revealed - self.mines)
        if revealed_safe >= safe_cells:
            self._finish("won")

    def _finish(self, reason: str) -> None:
        if self.finished:
            return
        self.finished = True
        self.end_reason = reason
        self.finished_at = time.monotonic()
        self.score = self._calculate_score(final=True)

    def elapsed_sec(self) -> int:
        if self.started_at is None:
            return 0
        end_time = self.finished_at if self.finished_at is not None else time.monotonic()
        return int(max(0.0, end_time - self.started_at))

    def time_left_sec(self) -> int:
        return max(0, self.time_limit_sec - self.elapsed_sec())

    def _calculate_score(self, final: bool) -> int:
        revealed_safe = len(self.revealed - self.mines)
        correct_flags = len(self.flags & self.mines)
        wrong_flags = len(self.flags - self.mines)

        value = revealed_safe * 10 + correct_flags * 4 - wrong_flags * 2

        if final:
            if self.end_reason == "won":
                value += self.time_left_sec() * 5
            elif self.end_reason == "lost":
                value -= 25
            elif self.end_reason == "time":
                value -= 10

        return max(0, int(value))

    def _cell_value(self, x: int, y: int) -> int:
        coord = (x, y)
        if coord in self.revealed:
            if coord in self.mines:
                return -1
            return self._adjacent_mines(x, y)
        if coord in self.flags:
            return -3
        if self.finished and coord in self.mines:
            return -1
        return -2

    def status(self) -> str:
        if self.finished:
            if self.end_reason == "won":
                return "won"
            if self.end_reason == "lost":
                return "lost"
            return "time"
        if self.started_at is None:
            return "waiting"
        return "running"

    def public_state(self) -> dict:
        grid = [[self._cell_value(x, y) for x in range(self.width)] for y in range(self.height)]

        return {
            "width": self.width,
            "height": self.height,
            "mine_count": self.mine_count,
            "flag_count": len(self.flags),
            "revealed_count": len(self.revealed - self.mines),
            "grid": grid,
            "status": self.status(),
            "time_limit_sec": self.time_limit_sec,
            "elapsed_sec": self.elapsed_sec(),
            "time_left_sec": self.time_left_sec(),
            "exploded": list(self.exploded) if self.exploded else None,
            "score": self.score if self.finished else self._calculate_score(final=False),
        }
