using Farm.Configs;

namespace Farm.Products;

public class Wheat(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 1,
        DamageLevel2 = 3,
        DamageLevel3 = 6,
        BasePrice = 26
    };

    private void Clean()
    {
        Console.WriteLine("Пшеница очищена от примесей.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 4);
    }

    private void Package()
    {
        Console.WriteLine("Пшеница упакована в мешки.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }

    public override void HandleAfterCollection()
    {
        Clean();
        Package();
    }
}