using Farm.Configs;
using Farm.Warehouses;

namespace Farm.Products;

public abstract class Product(ProductConfig config)
{
    public int Amount => config.Amount;
    public int Freshness => config.Freshness;
    public decimal Price => config.BasePrice * FreshnessFactor();

    private decimal FreshnessFactor()
    {
        return config.Freshness / 100.0m;
    }

    public void Update()
    {
        if (config.Damage >= ProductConfig.LowDamageThreshold1)
            config.Freshness = Math.Max(0, config.Freshness - config.DamageLevel1);
        if (config.Damage >= ProductConfig.LowDamageThreshold2)
            config.Freshness = Math.Max(0, config.Freshness - config.DamageLevel2);
        if (config.Damage >= ProductConfig.LowDamageThreshold3)
            config.Freshness = Math.Max(0, config.Freshness - config.DamageLevel3);

        if (config.Freshness == 0) config.Amount = 0;
    }


    public void Produce(int productivity)
    {
        if (config.MaxAmount > config.Amount)
            config.Amount = Math.Clamp(config.Amount + productivity, 0, config.MaxAmount);
    }

    public virtual void HandleAfterCollection()
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}