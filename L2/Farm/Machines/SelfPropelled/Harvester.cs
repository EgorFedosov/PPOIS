using Farm.Interfaces;
using Farm.Exceptions;
namespace Farm.Machines.SelfPropelled;

public class Harvester(string? name) : Machine(name ?? "Harvester")
{
    public override void AssignDriver(IWorker? driver)
    {
        if (driver?.GetType().Name != "EquipmentOperator")
            throw new InvalidHarvesterDriverException("Только оператор техники может управлять комбайном!");
        base.AssignDriver(driver);
    }

    protected virtual void TurnOn()
    {
        if (Driver == null || Driver.GetType().Name != "EquipmentOperator")
            throw new HarvesterCannotBeTurnedOnException("Только оператор техники может включить комбайн!");
    }
}