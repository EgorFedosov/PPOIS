namespace AirportSystem.Domain.Exceptions.Money;

/// <summary>Отрицательная сумма.</summary>
public class NegativeMoneyAmountException(decimal amount) :
    Exception($"Сумма не может быть отрицательной: {amount}");