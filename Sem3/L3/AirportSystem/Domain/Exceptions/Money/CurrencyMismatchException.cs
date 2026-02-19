using AirportSystem.Domain.Enums;

namespace AirportSystem.Domain.Exceptions.Money;

/// <summary>Разные валюты.</summary>
public sealed class CurrencyMismatchException(Currency left, Currency right)
    : Exception($"Несовпадение валют: {left} vs {right}");