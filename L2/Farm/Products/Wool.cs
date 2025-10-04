using Farm.Configs;

namespace Farm.Products;

public class Wool(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 1,
        DamageLevel2 = 2,
        DamageLevel3 = 4,
        BasePrice = 45
    };

    public void Card()
    {
        Console.WriteLine("Шерсть вычесана.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 6);
    }

    public void Wash()
    {
        Console.WriteLine("Шерсть промыта.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 2);
    }
}