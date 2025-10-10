using Farm.Configs;
using Farm.Exceptions;
using Farm.Interfaces;
using Farm.Places;

namespace Farm.Employees;

public abstract class Employee : IWorker
{
    private readonly EmployeeConfig _config;
    private Place? _location;

    protected Employee(EmployeeConfig config)
    {
        _config = config;
        Console.WriteLine($"{_config.Name} создан(а).");

        if (config.Location != null)
        {
            Location = config.Location;
        }
    }

    private Place? Location
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

    public string? GetName() => _config.Name;

    public int GetWorkCount() => _config.WorkCount;

    public void ResetWorkCount() => _config.WorkCount = 0;

    public EmployeeLevel GetLevel() => _config.Level;

    public void SetLevel(EmployeeLevel level) => _config.Level = level;



    public void ReceiveSalary(decimal amount) => _config.Balance += amount < 0 ? 0 : amount;

    public abstract void Work();

    public abstract void StopWork();
}