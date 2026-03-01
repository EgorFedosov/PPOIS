"""Domain entities for cooking simulation."""

from dataclasses import dataclass, field
from enum import Enum

from ..exceptions import EggError


class EggState(str, Enum):
    RAW = "raw"
    FRIED = "fried"


@dataclass
class Stove:
    """Provides heat for cooking."""

    is_on: bool = False

    def turn_on(self) -> None:
        self.is_on = True

    def turn_off(self) -> None:
        self.is_on = False


@dataclass
class Pan:
    """Container for ingredients that can be heated."""

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
    """Tool for mixing pan contents."""

    def mix(self, pan: Pan) -> None:
        pan.contents = list(reversed(pan.contents))


@dataclass
class Egg:
    """Main ingredient with simple lifecycle."""

    is_broken: bool = False
    state: EggState = EggState.RAW

    def break_egg(self) -> None:
        self.is_broken = True

    def fry(self) -> None:
        if not self.is_broken:
            raise EggError("Яйцо нужно разбить перед жаркой.")
        self.state = EggState.FRIED


@dataclass(frozen=True)
class Oil:
    """Marker entity for ingredient."""

    name: str = "oil"


@dataclass(frozen=True)
class Spices:
    """Marker entity for ingredient."""

    name: str = "spices"
