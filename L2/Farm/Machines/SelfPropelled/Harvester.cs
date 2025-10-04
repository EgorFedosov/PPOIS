using Farm.Interfaces;
using Farm.Fields;

namespace Farm.Machines.SelfPropelled;

public class Harvester : Machine
{
    public override void AssignDriver(IWorker driver)
    {
        // TODO кастомное исключение
        if (driver.GetType().Name != "EquipmentOperator")
            throw new InvalidOperationException("Только оператор техники может управлять комбайном!");
        base.AssignDriver(driver);
    }

    public override void TurnOn()
    {
        // TODO кастомное исключение
        if (Driver == null || Driver.GetType().Name != "EquipmentOperator")
            throw new InvalidOperationException("Только оператор техники может включить комбайн!");
        base.TurnOn();
    }
}