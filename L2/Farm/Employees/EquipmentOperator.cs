using Farm.Configs;
using Farm.Fields;
using Farm.Exceptions;
using Farm.Warehouses;
using Farm.Interfaces;

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
    public IMachine? CurrentMachine { get; private set; }

    public void SitInMachine(IMachine machine)
    {
        if (machine.Driver != null)
            throw new MachineAlreadyOccupiedException("Техника занята");
        if (_config.Location == null)
            throw new InvalidEmployeeLocationException("Рабочее место не задано");
        if (machine.Location != _config.Location)
            throw new MachineLocationMismatchException("Машина и работник находятся в разных местах");
        Console.WriteLine($"{_config.Name} сел в {machine.Name}");
        machine.AssignDriver(this);
        CurrentMachine = machine;
    }

    private void LeaveMachine()
    {
        if (CurrentMachine == null)
            throw new DriverNotAssignedException("Водитель не назначен");

        CurrentMachine.AssignDriver(null);
        CurrentMachine = null;
    }

    public override void Work()
    {
        if (Warehouse == null)
            throw new WarehouseNotAssignedException("Склад не установлен");
        if (CurrentMachine == null || _config.Location == null)
            throw new EquipmentOrFieldNotAssignedException("Не задана техника или поле");
        if (CurrentMachine.Driver != this)
            throw new DriverNotAssignedException("Водитель не назначен");
        if (!CurrentMachine.IsOn)
            throw new MachineNotOnException("Техника выключена");

        if (_config.Location is not Field field)
            throw new InvalidWorkLocationException("Оператор может работать только на поле");

        var harvested = field.CollectProduct(Warehouse);
        if (!harvested && !field.TryPlantFromWarehouse(Warehouse))
            throw new InsufficientSeedsException("Недостаточно семян или поле заполнено");
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