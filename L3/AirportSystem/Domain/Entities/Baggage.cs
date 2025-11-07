namespace AirportSystem.Domain.Entities;

public class Baggage : IEquatable<Baggage>
{
    public double WeightKg { get; }
    private Guid BaggageId { get; } = Guid.NewGuid();

    protected Baggage(double weightKg)
    {
        if (weightKg <= 0)
            throw new ArgumentException("Вес багажа должен быть положительным.", nameof(weightKg));
        WeightKg = weightKg;
    }

    public bool Equals(Baggage? other)
    {
        if (other is null) return false;
        return BaggageId == other.BaggageId;
    }

    public override bool Equals(object? obj)
    {
        return obj is Baggage baggage && Equals(baggage);
    }

    public override int GetHashCode()
    {
        return BaggageId.GetHashCode();
    }
}