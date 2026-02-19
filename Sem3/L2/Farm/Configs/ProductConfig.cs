namespace Farm.Configs;

public class ProductConfig
{
    public const int LowDamageThreshold1 = 30;
    public const int LowDamageThreshold2 = 60;
    public const int LowDamageThreshold3 = 90;
    private const int DefaultMaxAmount = 100;
    private int _amount;

    private int _damage;

    private int _freshness = 100;

    public int Amount
    {
        get => _amount;
        set => _amount = Math.Clamp(value, 0, MaxAmount);
    }

    public int MaxAmount { get; init; } = DefaultMaxAmount;
    public decimal BasePrice { get; init; }

    public int Freshness
    {
        get => _freshness;
        set => _freshness = Math.Clamp(value, 0, 100);
    }

    public int Damage
    {
        get => _damage;
        set => _damage = Math.Clamp(value, 0, 100);
    }

    public int DamageLevel1 { get; init; }
    public int DamageLevel2 { get; init; }
    public int DamageLevel3 { get; init; }
    public bool Processed { get; set; }
}