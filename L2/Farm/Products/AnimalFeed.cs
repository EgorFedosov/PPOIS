namespace Farm.Products;

public class AnimalFeed(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 1,
        DamageLevel2 = 3,
        DamageLevel3 = 7
    };

    public void Mix()
    {
        Console.WriteLine("Корм смешан.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 3);
    }

    public void StoreCool()
    {
        Console.WriteLine("Корм помещен в прохладное место.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }
}