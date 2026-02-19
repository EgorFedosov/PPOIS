using Farm.Configs;

namespace Farm.Products;

public class Corn(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 1,
        DamageLevel2 = 4,
        DamageLevel3 = 8,
        BasePrice = 12
    };

    private void Husk()
    {
        Console.WriteLine("Кукуруза очищена от листьев.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
    }

    private void Roast()
    {
        Console.WriteLine("Часть кукурузы слегка обжарена.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Max(0, effectiveConfig.Freshness - 3);
        effectiveConfig.Damage = Math.Min(100, effectiveConfig.Damage + 5);
    }

    public override void HandleAfterCollection()
    {
        Husk();
        Roast();
    }
}