using Farm.Products;

namespace Farm.Warehouses
{
    public class Warehouse()
    {
        private readonly Dictionary<Product, uint> _products = new Dictionary<Product, uint>();

        public void Store(Product product)
        {
            _products[product] += (uint)product.Amount;

            var seed = new CropSeed();
            _products.TryAdd(seed, 0);
            _products[seed] += (uint)Math.Floor(product.Amount * 0.1);
        }

        public bool UseSeeds(uint count)
        {
            var seed = new CropSeed();
            if (_products.TryGetValue(seed, out var amount) && amount >= count)
            {
                _products[seed] = amount - count;
                return true;
            }

            return false;
        }
    }
}