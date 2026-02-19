using Farm.Configs;
using Farm.Exceptions;
using Farm.Interfaces;
using Farm.Places;

namespace Farm.Employees;

public abstract class Employee : IWorker
{
    private readonly EmployeeConfig _config;
    private Place? _location;
    public string Name => _config.Name;
    public int  Age => _config.Age;
 

    protected Employee(EmployeeConfig config)
    {
        _config = config;
        Console.WriteLine($"{_config.Name} создан(а).");

        if (config.Location != null)
        {
            Location = config.Location;
        }
    }

    public Place? Location
    {
        get => _location;
        set
        {
            if (_location == value) return;

            _location?.RemoveEntity(this);
            _location = value;
            _config.Location = value;
            _location?.AddEntity(this);
        }
    }

    public void MoveTo(Place newPlace)
    {
        if (Location == newPlace)
            throw new InvalidMoveException($"{_config.Name} уже находится на {newPlace.Name}");

        Location = newPlace;
    }

    public int WorkCount() => _config.WorkCount;

    public void ResetWorkCount() => _config.WorkCount = 0;

    public EmployeeLevel Level
    {
        get => _config.Level;
        set => _config.Level = value;
    }
    
    public void ReceiveSalary(decimal amount) => _config.Balance += amount < 0 ? 0 : amount;

    public abstract void Work();

    public abstract void StopWork();
}