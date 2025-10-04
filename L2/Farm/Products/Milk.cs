using Farm.Configs;
namespace Farm.Products;

public class Milk(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 2,
        DamageLevel2 = 4,
        DamageLevel3 = 8
    };

    public void Pasteurize()
    {
        Console.WriteLine("Молоко пастеризовано.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 10);
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 2);
    }

    public void Chill()
    {
        Console.WriteLine("Молоко охлаждено для хранения.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
    }
}