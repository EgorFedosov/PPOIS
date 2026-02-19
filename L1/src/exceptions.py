"""Custom exceptions for cooking domain and CLI flow."""


class CookingError(Exception):
    """Base exception for all cooking-related errors."""


class EggError(CookingError):
    """Raised for egg-specific invalid actions."""


class InvalidStateTransitionError(CookingError):
    """Raised when an operation is not allowed in the current state."""
