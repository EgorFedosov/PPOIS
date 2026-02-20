"""Доменные сущности для симуляции процесса готовки."""

from dataclasses import dataclass, field
from enum import Enum

from src.exceptions import EggError


class EggState(str, Enum):
    RAW = "raw"
    FRIED = "fried"


@dataclass
class Stove:
    """Обеспечивает нагрев для приготовления."""

    is_on: bool = False

    def turn_on(self) -> None:
        self.is_on = True

    def turn_off(self) -> None:
        self.is_on = False


@dataclass
class Pan:
    """Контейнер для ингредиентов, который может нагреваться."""

    is_hot: bool = False
    contents: list[object] = field(default_factory=list)

    def add(self, ingredient: object) -> None:
        self.contents.append(ingredient)

    def clear(self) -> None:
        self.contents.clear()

    def heat(self) -> None:
        self.is_hot = True

    def cool(self) -> None:
        self.is_hot = False


@dataclass
class Spatula:
    """Инструмент для перемешивания содержимого сковороды."""

    def mix(self, pan: Pan) -> None:
        pan.contents = list(reversed(pan.contents))


@dataclass
class Egg:
    """Основной продукт с простым жизненным циклом."""

    is_broken: bool = False
    state: EggState = EggState.RAW

    def break_egg(self) -> None:
        self.is_broken = True

    def fry(self) -> None:
        if not self.is_broken:
            raise EggError("Egg must be broken before frying.")
        self.state = EggState.FRIED


@dataclass(frozen=True)
class Oil:
    """Сущность-маркер ингредиента."""

    name: str = "oil"


@dataclass(frozen=True)
class Spices:
    """Сущность-маркер ингредиента."""

    name: str = "spices"
