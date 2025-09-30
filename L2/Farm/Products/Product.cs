namespace Farm.Products;

public abstract class Product(int maxAmount = 100)
{
    // TODO добавить уникальное поведение для каждого продукта(например сделать муку из зерна)
    private int _amount;
    public int Amount => _amount;
    private bool IsFull => _amount >= maxAmount;


    public void Produce(int productivity)
    {
        if (!IsFull)
            _amount = Math.Clamp(_amount + productivity, 0, maxAmount);
    }
    public void Reset() => _amount = 0;
}