using Farm.Interfaces;
using Farm.Configs;
using Farm.Places;
using Farm.Warehouses;

namespace Farm.Employees;

public abstract class Employee(EmployeeConfig config) : IWorker
{
    public abstract void Work(Warehouse warehouse);
    public abstract void StopWork();

    public void MoveTo(Place? newPlace)
    {
        if (newPlace == null || config.Location == newPlace) return;
        //TODO добавить кастомное исключение
        config.Location?.RemoveEntity(this);
        newPlace.AddEntity(this);
        config.Location = newPlace;
    }
}