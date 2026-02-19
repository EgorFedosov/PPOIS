namespace AirportSystem.Domain.ValueObjects;

/// <summary>
/// Технические характеристики самолета.
/// </summary>
public sealed class AirplaneSpecs : IEquatable<AirplaneSpecs>
{
    public string Manufacturer { get; }
    public int ModelYear { get; }
    public string EngineType { get; }

    public AirplaneSpecs(string manufacturer, int modelYear, string engineType)
    {
        Manufacturer = manufacturer;
        ModelYear = modelYear;
        EngineType = engineType;
    }

    public bool Equals(AirplaneSpecs? other)
    {
        if (other is null) return false;
        return Manufacturer == other.Manufacturer && ModelYear == other.ModelYear && EngineType == other.EngineType;
    }

    public override bool Equals(object? obj) => obj is AirplaneSpecs other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Manufacturer, ModelYear, EngineType);
}