using Farm.Configs;

namespace Farm.Products;

public class Potato(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 1,
        DamageLevel2 = 3,
        DamageLevel3 = 6,
        BasePrice = 4
    };

    private void Brush()
    {
        Console.WriteLine("Картофель очищен от земли.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 3);
    }

    private void StoreCool()
    {
        Console.WriteLine("Картофель помещен в прохладное хранилище.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 7);
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }

    public override void HandleAfterCollection()
    {
        Brush();
        StoreCool();
    }
}