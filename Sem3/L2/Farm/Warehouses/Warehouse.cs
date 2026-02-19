using Farm.Places;
using Farm.Products;

namespace Farm.Warehouses;

public class Warehouse : Place
{
    private static readonly CropSeed SeedKey = new();
    private readonly Dictionary<Product, uint> _products = new();

    public Warehouse(string? name) : base(name)
    {
        _products[SeedKey] = 10000;
    }
    

    public void Store(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        product.HandleAfterCollection();

        _products[product] = _products.GetValueOrDefault(product) + (uint)product.Amount;
        _products[SeedKey] += (uint)Math.Floor(product.Amount * 0.1);
    }


    public uint TakeSeeds(uint requestedCount)
    {
        if (!_products.TryGetValue(SeedKey, out var available) || available == 0)
            return 0;

        var taken = Math.Min(requestedCount, available);
        _products[SeedKey] = available - taken;
        return taken;
    }

    public IEnumerable<Product> GetAvailableProductsForSale()
    {
        return _products
            .Where(kv => kv.Value > 0 && kv.Key.GetType() != typeof(CropSeed))
            .Select(kv => kv.Key);
    }

    public bool TakeProduct(Product product, uint count)
    {
        if (!_products.TryGetValue(product, out var available) || available < count)
        {
            return false;
        }
        
        _products[product] = available - count;
        return true;
    }
}