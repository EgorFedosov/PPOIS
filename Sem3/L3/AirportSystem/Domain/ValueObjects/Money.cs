using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Exceptions.Money;

namespace AirportSystem.Domain.ValueObjects;

/// <summary> Value Object, хранит тип валюты и количество /// </summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new NegativeMoneyAmountException(amount);
        Amount = amount;
        Currency = currency;
    }

    public void Print()
    {
        Console.WriteLine($"Amount : {Amount}, Currency: {Currency}");
    }

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (Currency != other.Currency)
            throw new CurrencyMismatchException(Currency, other.Currency);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (Currency != other.Currency)
            throw new CurrencyMismatchException(Currency, other.Currency);
        if (Amount < other.Amount)
            throw new NotEnoughMoneyException(Amount, other.Amount);
        return new Money(Amount - other.Amount, Currency);
    }

    public bool Equals(Money? other)
        => other is not null
           && Amount == other.Amount
           && Currency == other.Currency;

    public override bool Equals(object? obj)
        => obj is Money money && Equals(money);

    public override int GetHashCode()
        => HashCode.Combine(Amount, Currency);

    public static bool operator ==(Money? left, Money? right)
        => Equals(left, right);

    public static bool operator !=(Money? left, Money? right)
        => !Equals(left, right);

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException();
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException();
        return new Money(a.Amount - b.Amount, a.Currency);
    }
}