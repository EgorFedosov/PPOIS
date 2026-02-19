using Farm.Configs;

namespace Farm.Products;

public class CropSeed(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new()
    {
        DamageLevel1 = 1,
        DamageLevel2 = 2,
        DamageLevel3 = 3,
        BasePrice = 14
    };

    private void Sort()
    {
        Console.WriteLine("Семена отсортированы по качеству.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 2);
    }

    private void Dry()
    {
        Console.WriteLine("Семена подсушены.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 1);
    }
    
    public override void HandleAfterCollection()
    {
        Sort();
        Dry();
    }
}