using Farm.Interfaces;
using Farm.Places;
using Farm.Exceptions;

namespace Farm.Machines;

public abstract class Machine : IMachine
{
    private Place? _location;

    protected Machine(string name, Place? initialLocation = null)
    {
        Name = name;
        if (initialLocation != null)
        {
            Location = initialLocation;
        }
    }

    public IWorker? Driver { get; private set; }
    public string Name { get; }
    public bool IsOn { get; private set; }

    public Place? Location
    {
        get => _location;
        private set
        {
            if (_location == value) return;

            _location?.RemoveEntity(this);
            _location = value;
            _location?.AddEntity(this);
        }
    }

    public virtual void MoveTo(Place newPlace)
    {
        Location = newPlace;
    }

    public virtual void AssignDriver(IWorker? driver)
    {
        Driver = driver;
    }

    public virtual void DriveTo(Place destination)
    {
        if (Driver == null)
            throw new NoDriverAssignedException("Нет водителя!");
        if (!IsOn)
            throw new MachineNotOnException("Машина выключена!");
        
        Location = destination;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{Name} завелся(ась)");
        IsOn = true;
    }
    
    public void TurnOff()
    {
        IsOn = false;
    }
}