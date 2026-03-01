"""Factories for creating the default domain model."""

from .models.cooking_process import CookingProcess
from .models.entities import Egg, Oil, Pan, Spatula, Spices, Stove


def create_default_process() -> CookingProcess:
    """Create a fresh cooking process with default entities."""

    return CookingProcess(
        stove=Stove(),
        pan=Pan(),
        spatula=Spatula(),
        egg=Egg(),
        oil=Oil(),
        spices=Spices(),
    )

