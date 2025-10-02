namespace Farm.Products;

public class Wheat(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 1,
        DamageLevel2 = 3,
        DamageLevel3 = 6
    };

    public void Clean()
    {
        Console.WriteLine("Пшеница очищена от примесей.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 4);
    }

    public void Package()
    {
        Console.WriteLine("Пшеница упакована в мешки.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }
}