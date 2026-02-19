using Farm.Configs;

namespace Farm.Products;

public class Fruit(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 3,
        DamageLevel2 = 7,
        DamageLevel3 = 15,
        BasePrice = 11
    };

    private void Wash()
    {
        Console.WriteLine("Фрукты промыты.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 8);
    }

    private void Ripen()
    {
        Console.WriteLine("Фрукты дозревают на складе.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage + 2);
    }

    public override void HandleAfterCollection()
    {
        Wash();
        Ripen();
    }
}