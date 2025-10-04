using Farm.Places;
using Farm.Interfaces;

namespace Farm.Machines;

public abstract class Machine : IMachine
{
    public IWorker? Driver { get; private set; }
    public string? Name { get; protected set; }
    public Place? Location { get; private set; }
    public bool IsOn { get; set; } = false;

    public virtual void MoveTo(Place newPlace) => Location = newPlace;
    public virtual void AssignDriver(IWorker? driver) => Driver = driver;

    public virtual void DriveTo(Place destination)
    {
        if (Driver == null)
            throw new InvalidOperationException("Нет водителя!");
        if (!IsOn)
            throw new InvalidOperationException("Машина выключена!");
        Location = destination;
    }

    public virtual void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;
}