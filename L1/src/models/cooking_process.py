"""Контроллер процесса готовки с конечным автоматом."""

from dataclasses import dataclass
from enum import Enum

from src.exceptions import InvalidStateTransitionError
from .entities import Egg, EggState, Oil, Pan, Spatula, Spices, Stove


class CookingState(str, Enum):
    """Состояния процесса приготовления."""

    IDLE = "idle"
    HEATING = "heating"
    COOKING = "cooking"
    SEASONING = "seasoning"
    FINISHING = "finishing"
    SERVED = "served"


@dataclass
class CookingProcess:
    """Сценарий приготовления яичницы."""

    stove: Stove
    pan: Pan
    spatula: Spatula
    egg: Egg
    oil: Oil
    spices: Spices
    state: CookingState = CookingState.IDLE
    oil_added: bool = False
    spices_added: bool = False
    is_mixed: bool = False

    def heat_pan(self) -> str:
        """Операция разогрева сковороды."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.pan.is_hot or self.state == CookingState.HEATING:
            return "Сковорода уже горячая."
        if self.state != CookingState.IDLE:
            raise InvalidStateTransitionError(
                "Нельзя разогревать сковороду после начала готовки."
            )
        self.stove.turn_on()
        self.pan.heat()
        self.state = CookingState.HEATING
        return "Сковорода разогрета."

    def break_eggs(self) -> str:
        """Операция разбивания яиц."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.egg.is_broken:
            return "Яйца уже разбиты."
        if not self.pan.is_hot:
            raise InvalidStateTransitionError(
                "Сковорода холодная. Сначала разогрейте сковороду."
            )
        if self.state != CookingState.HEATING:
            raise InvalidStateTransitionError("Разбить яйца можно только после разогрева.")
        self.egg.break_egg()
        if self.egg not in self.pan.contents:
            self.pan.add(self.egg)
        self.state = CookingState.COOKING
        return "Яйца разбиты."

    def add_oil(self) -> str:
        """Добавление масла на сковороду."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.oil_added:
            return "Масло уже добавлено."
        if self.state not in (
            CookingState.HEATING,
            CookingState.COOKING,
            CookingState.SEASONING,
        ):
            raise InvalidStateTransitionError(
                "Масло можно добавить только во время разогрева или готовки."
            )
        self.pan.add(self.oil)
        self.oil_added = True
        return "Масло добавлено."

    def add_spices(self) -> str:
        """Операция добавления приправ."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.spices_added:
            return "Приправы уже добавлены."
        if self.state not in (CookingState.COOKING, CookingState.SEASONING):
            raise InvalidStateTransitionError(
                "Приправы можно добавить после разбивания яиц."
            )
        self.pan.add(self.spices)
        self.spices_added = True
        self.state = CookingState.SEASONING
        return "Приправы добавлены."

    def fry_eggs(self) -> str:
        """Операция обжаривания яиц."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.egg.state == EggState.FRIED:
            return "Яйца уже обжарены."
        if self.state not in (CookingState.COOKING, CookingState.SEASONING):
            raise InvalidStateTransitionError("Нельзя жарить яйца из текущего состояния.")
        self.egg.fry()
        self.state = CookingState.FINISHING
        return "Яйца обжарены."

    def mix_and_serve(self) -> str:
        """Операция перемешивания и подачи блюда."""

        if self.state == CookingState.SERVED:
            return "Блюдо уже подано."
        if self.state != CookingState.FINISHING:
            raise InvalidStateTransitionError("Подача доступна только после обжаривания.")
        if self.egg.state != EggState.FRIED:
            raise InvalidStateTransitionError("Нельзя подавать сырое блюдо.")
        self.spatula.mix(self.pan)
        self.is_mixed = True
        self.stove.turn_off()
        self.pan.cool()
        self.state = CookingState.SERVED
        return "Блюдо перемешано и подано."

    def snapshot(self) -> dict[str, object]:
        """Вернуть сериализуемый снимок состояния."""

        return {
            "state": self.state.value,
            "oil_added": self.oil_added,
            "spices_added": self.spices_added,
            "is_mixed": self.is_mixed,
            "stove": {"is_on": self.stove.is_on},
            "pan": {
                "is_hot": self.pan.is_hot,
                "contents": [type(item).__name__ for item in self.pan.contents],
            },
            "egg": {
                "is_broken": self.egg.is_broken,
                "state": self.egg.state.value,
            },
        }

    @classmethod
    def from_snapshot(cls, data: dict[str, object]) -> "CookingProcess":
        """Восстановить процесс из снимка."""

        stove_data = data.get("stove", {})
        pan_data = data.get("pan", {})
        egg_data = data.get("egg", {})

        if not isinstance(stove_data, dict):
            stove_data = {}
        if not isinstance(pan_data, dict):
            pan_data = {}
        if not isinstance(egg_data, dict):
            egg_data = {}

        stove = Stove(is_on=bool(stove_data.get("is_on", False)))
        pan = Pan(is_hot=bool(pan_data.get("is_hot", False)))
        egg_state_raw = egg_data.get("state", EggState.RAW.value)
        try:
            egg_state = EggState(egg_state_raw)
        except ValueError:
            egg_state = EggState.RAW

        egg = Egg(
            is_broken=bool(egg_data.get("is_broken", False)),
            state=egg_state,
        )

        restored = cls(
            stove=stove,
            pan=pan,
            spatula=Spatula(),
            egg=egg,
            oil=Oil(),
            spices=Spices(),
            state=cls._parse_state(data.get("state")),
            oil_added=bool(data.get("oil_added", False)),
            spices_added=bool(data.get("spices_added", False)),
            is_mixed=bool(data.get("is_mixed", False)),
        )

        for item_name in pan_data.get("contents", []):
            if item_name == "Oil":
                restored.pan.add(restored.oil)
            elif item_name == "Egg":
                restored.pan.add(restored.egg)
            elif item_name == "Spices":
                restored.pan.add(restored.spices)

        return restored

    @staticmethod
    def _parse_state(raw_state: object) -> CookingState:
        if not isinstance(raw_state, str):
            return CookingState.IDLE
        try:
            return CookingState(raw_state)
        except ValueError:
            return CookingState.IDLE
