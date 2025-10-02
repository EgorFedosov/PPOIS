namespace Farm.Products;

public class Cabbage(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 2,
        DamageLevel2 = 5,
        DamageLevel3 = 10
    };

    public void PrepareForStorage()
    {
        Console.WriteLine("Капуста подготовлена к хранению.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 10);
    }
}