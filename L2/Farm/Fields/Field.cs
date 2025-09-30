using Farm.Places;
using Farm.Products;
using Farm.Interface;

namespace Farm.Fields;

public abstract class Field(FieldConfig config) : Place
{
    public Product? Product => config.Product;

    public void Update()
    {
        config.Productivity = (int)Math.Round(CalculateProductivity());
        config.Product?.Produce(config.Productivity);
    }

    public void Plant(int seeds)
    {
        //TODO добавить кастомное исключение 
        if (config.Product == null || config.Product.Amount == 0)
            throw new Exception();
        config.SeedCount = seeds;
    }

    public void Care(IWorker worker)
    {
        config.SoilCareLevel = Math.Clamp(
            config.SoilCareLevel + worker.CarePower,
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