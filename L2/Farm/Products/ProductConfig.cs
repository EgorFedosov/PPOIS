namespace Farm.Products;

public class ProductConfig
{
    private const int DefaultMaxAmount = 100;
    private int _amount;

    public int Amount
    {
        get => _amount;
        set => _amount = Math.Clamp(value, 0, MaxAmount);
    }

    public int MaxAmount { get; init; } = DefaultMaxAmount;

    private int _freshness = 100;

    public int Freshness
    {
        get => _freshness;
        set => _freshness = Math.Clamp(value, 0, 100);
    }

    private int _damage = 0;

    public int Damage
    {
        get => _damage;
        set => _damage = Math.Clamp(value, 0, 100);
    }

    public int DamageLevel1 { get; init; }
    public int DamageLevel2 { get; init; }
    public int DamageLevel3 { get; init; }
    public int LowDamageThreshold1 { get; init; } = 30;
    public int LowDamageThreshold2 { get; init; } = 60;
    public int LowDamageThreshold3 { get; init; } = 90;
    public bool Processed { get; set; } = false;
}