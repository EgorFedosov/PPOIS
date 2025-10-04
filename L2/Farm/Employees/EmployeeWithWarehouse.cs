using Farm.Configs;
using Farm.Warehouses;

namespace Farm.Employees;

public abstract class EmployeeWithWarehouse(EmployeeConfig config, Warehouse warehouse) : Employee(config)
{
    protected Warehouse Warehouse { get; } = warehouse;
}