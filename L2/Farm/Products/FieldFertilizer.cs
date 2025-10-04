using Farm.Configs;
namespace Farm.Products;

public class FieldFertilizer(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    // получаем при уборке place от animal
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 1,
        DamageLevel2 = 1,
        DamageLevel3 = 2
    };

    public void Mix()
    {
        Console.WriteLine("Удобрение смешано.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 5);
    }

    public void StoreDry()
    {
        Console.WriteLine("Удобрение хранится в сухом месте.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }
}