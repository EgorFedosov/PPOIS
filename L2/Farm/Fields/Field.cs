using Farm.Configs;
using Farm.Places;
using Farm.Products;
using Farm.Warehouses;

namespace Farm.Fields;

public abstract class Field(FieldConfig config) : Place
{
    public Product? Product => config.Product;

    public void Update()
    {
        config.Productivity = (int)Math.Round(CalculateProductivity());
        config.Product?.Produce(config.Productivity);
    }

    public bool CollectProduct(Warehouse warehouse)
    { //TODO как то обработать
        if (config.Product == null || config.Product.Amount == 0)
            return false;
        config.Product.Collect(warehouse);
        return true;
    }

    public void Plant(int seeds)
    {
        //TODO добавить кастомное исключение 
        if (config.Product == null || config.Product.Amount == 0)
            throw new Exception();
        config.SeedCount = seeds;
    }

    public void Care(int carePower)
    {
        config.SoilCareLevel = Math.Clamp(
            config.SoilCareLevel + carePower,
            config.MinSoilCareLevel,
            config.MaxSoilCareLevel
        );
    }


    private double CalculateProductivity() =>
        (config.MinProductivity
         + (config.MaxProductivity - config.MinProductivity)
         * config.SoilCareLevel / 100.0)
        * Math.Min(1, (double)config.SeedCount / config.MaxSeedCount);
}