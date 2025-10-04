using Farm.Configs;
using Farm.Machines;
using Farm.Fields;
using Farm.Warehouses;

namespace Farm.Employees;

public class EquipmentOperator(EmployeeConfig? config = null) : Employee(config ?? DefaultConfig)
{
    private static readonly EmployeeConfig DefaultConfig = new EmployeeConfig
    {
        Name = "Equipment Operator",
        Age = 30,
    };

    public Machine? CurrentMachine { get; private set; }

    public void SitInMachine(Machine machine)
    {
        if (machine.Driver != null)
            throw new InvalidOperationException("Машина уже занята!");

        machine.AssignDriver(this);
        CurrentMachine = machine;
    }

    public void LeaveMachine()
    {
        if (CurrentMachine == null)
            throw new InvalidOperationException("Оператор не в машине!");

        CurrentMachine.AssignDriver(null);
        CurrentMachine = null;
    }

    public override void Work(Warehouse warehouse)
    {
        //  TODO свои искл

        if (CurrentMachine == null || config?.Location == null)
            throw new InvalidOperationException("Не задана техника или поле!");
        if (CurrentMachine.Driver != this)
            throw new InvalidOperationException("Оператор не назначен водителем!");
        if (!CurrentMachine.IsOn)
            throw new InvalidOperationException("Техника выключена!");

        if (config.Location is not Field field)
            throw new InvalidOperationException("Оператор может работать только на поле!");

        var collected = field.CollectProduct(warehouse);
        if (!collected && warehouse.UseSeeds(300))
        {
            field.Plant(300);
        }
    }

    public override void StopWork()
    {
        throw new NotImplementedException();
    }
}