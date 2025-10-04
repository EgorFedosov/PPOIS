using Farm.Configs;
using Farm.Interfaces;
using Farm.Places;

namespace Farm.Employees;

public abstract class Employee(EmployeeConfig config) : IWorker
{
    public int GetWorkCount() => config.WorkCount;

    public string? GetName() => config.Name;

    public void ResetWorkCount() => config.WorkCount = 0;

    public EmployeeLevel GetLevel() => config.Level;

    public void SetLevel(EmployeeLevel level) => config.Level = level;

    public abstract void Work();
    public abstract void StopWork();

    public void MoveTo(Place? newPlace)
    {
        if (newPlace == null || config.Location == newPlace) return;
        config.Location?.RemoveEntity(this);
        newPlace.AddEntity(this);
        config.Location = newPlace;
    }

    public void ReceiveSalary(decimal amount) => config.Balance += amount < 0 ? 0 : amount;
}