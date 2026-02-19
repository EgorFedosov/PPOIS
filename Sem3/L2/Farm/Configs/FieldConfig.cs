using Farm.Products;

namespace Farm.Configs;

public class FieldConfig
{
    private const uint MinSeedCount = 0;
    public const uint MaxSeedCount = 1000;
    public const int MaxSoilCareLevel = 100;

    private int _productivity;
    private uint _seedCount;
    private int _soilCareLevel;

    public int Productivity
    {
        get => _productivity;
        set => _productivity = Math.Clamp(value, MinProductivity, MaxProductivity);
    }

    public int SoilCareLevel
    {
        get => _soilCareLevel;
        set => _soilCareLevel = Math.Clamp(value, MinSoilCareLevel, MaxSoilCareLevel);
    }

    public uint SeedCount
    {
        get => _seedCount;
        set => _seedCount = Math.Clamp(value, MinSeedCount, MaxSeedCount);
    }

    public int MinProductivity { get; init; }
    public int MaxProductivity { get; init; }
    public int MinSoilCareLevel { get; init; }


    public string Name { get; init; } = "DefaultName";
    public Product? Product { get; init; }
}