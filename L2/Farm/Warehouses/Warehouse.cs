using Farm.Products;

namespace Farm.Warehouses;

public class Warehouse
{
    private readonly Dictionary<Product, uint> _products = new();

    public void Store(Product product)
    {
        _products[product] += (uint)product.Amount;

        var seed = new CropSeed();
        _products.TryAdd(seed, 0);
        _products[seed] += (uint)Math.Floor(product.Amount * 0.1);
    }

    public uint TakeSeeds(uint requestedCount)
    {
        var seed = new CropSeed();
        if (!_products.TryGetValue(seed, out var available) || available == 0)
            return 0;

        var taken = Math.Min(requestedCount, available);
        _products[seed] = available - taken;
        return taken;
    }

    public IEnumerable<Product> GetAvailableProductsForSale()
    {
        return _products.Keys
            .Where(p => _products[p] > 0 && p is not CropSeed);
    }


    public bool TakeProduct(Product product, uint count)
    {
        if (!_products.TryGetValue(product, out var available) || available < count)
            return false;

        _products[product] = available - count;
        return true;
    }
}