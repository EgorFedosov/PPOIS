using Farm.Configs;
using Farm.Products;
using Farm.Warehouses;

namespace Farm.Employees;

public class SalesManager(Warehouse warehouse, EmployeeConfig? config = null)
    : EmployeeWithWarehouse(config ?? DefaultConfig, warehouse)
{
    private static readonly EmployeeConfig DefaultConfig = new()
    {
        Name = "Sales Manager",
        Age = 28,
        Level = EmployeeLevel.Middle
    };

    private readonly EmployeeConfig _config = config ?? DefaultConfig;

    private decimal _moneyEarned;

    public override void Work()
    {
        foreach (var product in Warehouse.GetAvailableProductsForSale())
        {
            var quantity = (uint)product.Amount;
            if (quantity > 0)
                _moneyEarned += Sell(product, quantity);
        }

        _config.WorkCount++;
    }

    private decimal Sell(Product product, uint quantity)
    {
        if (Warehouse.TakeProduct(product, quantity))
        {
            var revenue = product.Price * quantity;
            _moneyEarned += revenue;
            Console.WriteLine($"Продано {quantity} {product.GetType().Name}, выручка: {revenue}");
            return revenue;
        }

        Console.WriteLine($"Не удалось продать {product.GetType().Name}, недостаточно на складе");
        return 0;
    }


    public override void StopWork() =>
        Console.WriteLine($"{_config.Name} завершил работу. Всего заработано: {_moneyEarned} денег.");
    
}