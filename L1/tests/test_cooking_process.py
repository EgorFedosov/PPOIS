import pytest

from src.exceptions import EggError, InvalidStateTransitionError
from src.models.cooking_process import CookingProcess, CookingState
from src.models.entities import Egg, EggState, Oil, Pan, Spatula, Spices, Stove


def make_process() -> CookingProcess:
    return CookingProcess(
        stove=Stove(),
        pan=Pan(),
        spatula=Spatula(),
        egg=Egg(),
        oil=Oil(),
        spices=Spices(),
    )


def test_happy_path_serves_fried_eggs() -> None:
    process = make_process()

    process.heat_pan()
    process.break_eggs()
    process.add_oil()
    process.add_spices()
    process.fry_eggs()
    process.mix_and_serve()

    assert process.state == CookingState.SERVED
    assert process.egg.state == EggState.FRIED
    assert process.stove.is_on is False
    assert process.pan.is_hot is False
    assert process.is_mixed is True


def test_cannot_break_eggs_when_pan_is_cold() -> None:
    process = make_process()

    with pytest.raises(InvalidStateTransitionError):
        process.break_eggs()


def test_cannot_add_spices_before_breaking_eggs() -> None:
    process = make_process()
    process.heat_pan()

    with pytest.raises(InvalidStateTransitionError):
        process.add_spices()


def test_snapshot_roundtrip_restores_state() -> None:
    process = make_process()
    process.heat_pan()
    process.break_eggs()
    process.add_oil()
    snapshot = process.snapshot()

    restored = CookingProcess.from_snapshot(snapshot)

    assert restored.state == CookingState.COOKING
    assert restored.stove.is_on is True
    assert restored.pan.is_hot is True
    assert restored.egg.is_broken is True
    assert restored.oil_added is True


def test_fry_egg_without_breaking_raises_custom_exception() -> None:
    egg = Egg()
    with pytest.raises(EggError):
        egg.fry()


def test_cannot_heat_pan_twice() -> None:
    process = make_process()
    process.heat_pan()
    message = process.heat_pan()
    assert message == "Сковорода уже горячая."


def test_repeated_additions_and_fry_show_clear_messages() -> None:
    process = make_process()
    process.heat_pan()
    process.break_eggs()

    assert process.add_oil() == "Масло добавлено."
    assert process.add_oil() == "Масло уже добавлено."

    assert process.add_spices() == "Приправы добавлены."
    assert process.add_spices() == "Приправы уже добавлены."

    assert process.fry_eggs() == "Яйца обжарены."
    assert process.fry_eggs() == "Яйца уже обжарены."


def test_all_actions_after_served_return_served_message() -> None:
    process = make_process()
    process.heat_pan()
    process.break_eggs()
    process.add_spices()
    process.fry_eggs()
    process.mix_and_serve()

    assert process.heat_pan() == "Блюдо уже подано."
    assert process.break_eggs() == "Блюдо уже подано."
    assert process.add_oil() == "Блюдо уже подано."
    assert process.add_spices() == "Блюдо уже подано."
    assert process.fry_eggs() == "Блюдо уже подано."
    assert process.mix_and_serve() == "Блюдо уже подано."
