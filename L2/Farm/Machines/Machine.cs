using Farm.Interfaces;
using Farm.Places;

namespace Farm.Machines;

public abstract class Machine(string name) : IMachine
{
    public IWorker? Driver { get; private set; }
    public string? Name { get; } = name;
    public Place? Location { get; private set; }
    public bool IsOn { get; private set; }

    public virtual void MoveTo(Place newPlace)
    {
        Location?.RemoveEntity(this);
        Location = newPlace;
        Location?.AddEntity(this);
    }

    public virtual void AssignDriver(IWorker? driver)
    {
        Driver = driver;
    }

    public virtual void DriveTo(Place destination)
    {
        if (Driver == null)
            throw new InvalidOperationException("Нет водителя!");
        if (!IsOn)
            throw new InvalidOperationException("Машина выключена!");
        Location = destination;
    }

    public void TurnOff()
    {
        IsOn = false;
    }
}