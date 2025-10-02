namespace Farm.Products;

public class Egg(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 2,
        DamageLevel2 = 5,
        DamageLevel3 = 10
    };

    public void CheckForCracks()
    {
        Console.WriteLine("Яйца проверены на трещины.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 2);
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }

    public void ChillEggs()
    {
        Console.WriteLine("Яйца охлаждены.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
    }
}