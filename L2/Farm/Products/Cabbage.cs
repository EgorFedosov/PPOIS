using Farm.Configs;

namespace Farm.Products;

public class Cabbage(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 2,
        DamageLevel2 = 5,
        DamageLevel3 = 10,
        BasePrice = 10
    };

    public void PrepareForStorage()
    {
        Console.WriteLine("Капуста подготовлена к хранению.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 10);
    }
}