using Farm.Interfaces;

namespace Farm.Places;

public abstract class Place
{
    private readonly List<IAnimal> _animals = new();
    private readonly List<IMachine> _machines = [];
    private readonly List<IWorker> _workers = new();
    private float Dirtiness { get; set; }

    public void AddEntity(IAnimal animal)
    {
        _animals.Add(animal);
    }

    public void RemoveEntity(IAnimal animal)
    {
        _animals.Remove(animal);
    }

    public void AddEntity(IMachine machine)
    {
        _machines.Add(machine);
    }

    public void RemoveEntity(IMachine machine)
    {
        _machines.Remove(machine);
    }

    public void AddEntity(IWorker worker)
    {
        _workers.Add(worker);
    }

    public void RemoveEntity(IWorker worker)
    {
        _workers.Remove(worker);
    }

    public IReadOnlyList<IAnimal> GetAnimals()
    {
        return _animals.AsReadOnly();
    }

    public IReadOnlyList<IMachine> GetMachines()
    {
        return _machines.AsReadOnly();
    }

    public IReadOnlyList<IWorker> GetWorkers()
    {
        return _workers.AsReadOnly();
    }

    public void IncreaseDirtiness(float amount)
    {
        Dirtiness = Math.Clamp(Dirtiness + amount, 0f, 100f);
    }
}