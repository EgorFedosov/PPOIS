namespace Farm.Products;

public abstract class Product(int maxAmount = 100)
{
    private int _amount;

    private int Amount
    {
        get => _amount;
        set => _amount = Math.Clamp(value, 0, maxAmount);
    }

    private bool IsFull => _amount >= maxAmount;


    public void Produce(int productivity)
    {
        if (!IsFull)
            Amount += productivity;
    }
}