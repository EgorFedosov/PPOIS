namespace Farm.Products;

public class Fruit(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 3,
        DamageLevel2 = 7,
        DamageLevel3 = 15
    };

    public void Wash()
    {
        Console.WriteLine("Фрукты промыты.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 8);
    }

    public void Ripen()
    {
        Console.WriteLine("Фрукты дозревают на складе.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage + 2);
    }
}