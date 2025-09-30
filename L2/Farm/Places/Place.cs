using Farm.Interface;

namespace Farm.Places;

public abstract class Place
{
    protected string? PlaceName { get; set; }
    protected uint PlaceId { get; set; }
    protected uint Area { get; set; }
    private float Dirtiness { get; set; }
    private readonly List<IAnimal> _animals = new();
    private readonly List<IMachine> _machines = new();
    private readonly List<IWorker> _workers = new();
    
    public void AddEntity(IAnimal animal) => _animals.Add(animal);
    public void RemoveEntity(IAnimal animal) => _animals.Remove(animal);
    public void AddEntity(IMachine machine) => _machines.Add(machine);
    public void RemoveEntity(IMachine machine) => _machines.Remove(machine);
    public void AddEntity(IWorker worker) => _workers.Add(worker);
    public void RemoveEntity(IWorker worker) => _workers.Remove(worker);
    
    public IReadOnlyList<IAnimal> GetAnimals() => _animals.AsReadOnly();
    public IReadOnlyList<IMachine> GetMachines() => _machines.AsReadOnly();
    public IReadOnlyList<IWorker> GetWorkers() => _workers.AsReadOnly();

    public void IncreaseDirtiness(float amount)
    {
        Dirtiness = Math.Clamp(Dirtiness + amount, 0f, 100f);
    }

    public void Clean()
    {
        Dirtiness = 0;
    }
}