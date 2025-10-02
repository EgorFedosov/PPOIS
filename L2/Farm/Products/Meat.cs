namespace Farm.Products;

public class Meat(ProductConfig? config = null) : Product(config ?? DefaultConfig)
{
    private static readonly ProductConfig DefaultConfig = new ProductConfig
    {
        DamageLevel1 = 2,
        DamageLevel2 = 5,
        DamageLevel3 = 10
    };

    public void Chill()
    {
        Console.WriteLine("Мясо охлаждено для хранения.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Freshness = Math.Min(100, effectiveConfig.Freshness + 8);
    }

    public void VacuumPack()
    {
        Console.WriteLine("Мясо упаковано в вакуум.");
        var effectiveConfig = config ?? DefaultConfig;
        effectiveConfig.Damage = Math.Max(0, effectiveConfig.Damage - 5);
    }
}