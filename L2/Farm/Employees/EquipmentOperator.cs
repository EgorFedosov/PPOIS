using Farm.Configs;
using Farm.Fields;
using Farm.Machines;
using Farm.Warehouses;

namespace Farm.Employees;

public class EquipmentOperator(Warehouse warehouse, EmployeeConfig? config = null)
    : EmployeeWithWarehouse(config ?? DefaultConfig, warehouse)
{
    private static readonly EmployeeConfig DefaultConfig = new()
    {
        Name = "Equipment Operator",
        Age = 30
    };

    private readonly EmployeeConfig _config = config ?? DefaultConfig;

    public string? Name => _config.Name;
    public Machine? CurrentMachine { get; private set; }

    public void SitInMachine(Machine machine)
    {
        if (machine.Driver != null)
            throw new InvalidOperationException("Машина уже занята!");
        if (_config.Location == null)
            throw new InvalidOperationException("Рабочее место не задано!");
        if (machine.Location != _config.Location)
            throw new InvalidOperationException("Машина не на том же поле!");

        machine.AssignDriver(this);
        CurrentMachine = machine;
    }

    private void LeaveMachine()
    {
        if (CurrentMachine == null)
            throw new InvalidOperationException("Оператор не в машине!");

        CurrentMachine.AssignDriver(null);
        CurrentMachine = null;
    }

    public override void Work()
    {
        if (Warehouse == null)
            throw new InvalidOperationException("Склад не установлен");
        if (CurrentMachine == null || _config.Location == null)
            throw new InvalidOperationException("Не задана техника или поле!");
        if (CurrentMachine.Driver != this)
            throw new InvalidOperationException("Оператор не назначен водителем!");
        if (!CurrentMachine.IsOn)
            throw new InvalidOperationException("Техника выключена!");

        if (_config.Location is not Field field)
            throw new InvalidOperationException("Оператор может работать только на поле!");

        var harvested = field.CollectProduct(Warehouse);
        if (!harvested && !field.TryPlantFromWarehouse(Warehouse))
            Console.WriteLine("Недостаточно семян или место на поле заполнено.");
    }

    public override void StopWork()
    {
        if (CurrentMachine != null)
        {
            if (CurrentMachine.IsOn)
            {
                CurrentMachine.TurnOff();
                Console.WriteLine($"{_config.Name} остановил машину {CurrentMachine.Name}");
            }

            LeaveMachine();
        }

        Console.WriteLine($"{_config.Name} завершил работу и покинул поле.");
    }
}