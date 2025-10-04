using Farm.Configs;
using Farm.Places;
using Farm.Warehouses;

namespace Farm.Fields;

public abstract class Field(FieldConfig config) : Place
{
    public string? Name => config.Name;

    public void Update()
    {
        config.Productivity = (int)Math.Round(CalculateProductivity());
        config.Product?.Produce(config.Productivity);
    }

    public bool CollectProduct(Warehouse warehouse)
    {
        if (config.Product == null || config.Product.Amount == 0)
            return false;

        config.Product.Collect(warehouse);
        return true;
    }

    private void Plant(uint count)
    {
        if (count == 0)
            return;

        if (config.SeedCount + count > FieldConfig.MaxSeedCount)
            throw new InvalidOperationException("Превышен лимит посева на поле!");

        config.SeedCount += count;
        Console.WriteLine($"Посеяно {count} семян. Всего: {config.SeedCount}/{FieldConfig.MaxSeedCount}");
    }

    public void Care(int carePower)
    {
        config.SoilCareLevel = Math.Clamp(
            config.SoilCareLevel + carePower,
            config.MinSoilCareLevel,
            FieldConfig.MaxSoilCareLevel
        );
    }

    public bool TryPlantFromWarehouse(Warehouse warehouse)
    {
        var availableSpace = FieldConfig.MaxSeedCount - config.SeedCount;
        if (availableSpace <= 0)
            return false;

        var seedsToPlant = warehouse.TakeSeeds(availableSpace);
        if (seedsToPlant == 0)
            return false;

        Plant(seedsToPlant);
        return true;
    }

    private double CalculateProductivity()
    {
        return (config.MinProductivity
                + (config.MaxProductivity - config.MinProductivity)
                * config.SoilCareLevel / 100.0)
               * Math.Min(1, (double)config.SeedCount / FieldConfig.MaxSeedCount);
    }
}