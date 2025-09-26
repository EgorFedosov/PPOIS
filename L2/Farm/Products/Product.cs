namespace Farm.Products;

public abstract class Product(int maxAmount)
{
    private int _amount = 0;

    private int Amount
    {
        get => _amount;
        set => _amount = Math.Clamp(value, 0, maxAmount);
    }

    public int MaxAmount => maxAmount;

    private bool IsFull => _amount >= maxAmount;
    

    public virtual void Produce(int productivity)
    {
        if (!IsFull)
            Amount += productivity;
    }
}