using Farm.Products;
using Farm.Interface;

namespace Farm.Fields;

public class FieldConfig
{
    private int _productivity;
    private int _soilCareLevel;
    private int _seedCount;

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

    public int SeedCount
    {
        get => _seedCount;
        set => _seedCount = Math.Clamp(value, MinSeedCount, MaxSeedCount);
    }

    public int MinProductivity { get; init; }
    public int MaxProductivity { get; init; }
    public int MaxSoilCareLevel { get; init; } = 100;
    public int MinSoilCareLevel { get; init; } = 0;
    private int MinSeedCount { get; init; } = 0;
    public int MaxSeedCount { get; init; } = 1000;

    public int SoilCareDecrement { get; init; }


    public string? Name { get; init; }
    public Product? Product { get; init; }

    public List<IMachine>? Machines { get; init; }
    public List<IWorker>? Workers { get; init; }
}