using Farm.Interfaces;

namespace Farm.Places;

public abstract class Place
{
    protected Place(string? name)
    {
        Name = name;
        Console.WriteLine($"{name} создан(о)");
    }

    public readonly string? Name;
    private readonly List<IAnimal> _animals = [];
    private readonly List<IMachine> _machines = [];
    private readonly List<IWorker> _workers = [];
    private float Dirtiness { get; set; }

    public void AddEntity(IAnimal animal) => _animals.Add(animal);
    public void RemoveEntity(IAnimal animal) => _animals.Remove(animal);

    public void AddEntity(IMachine machine) => _machines.Add(machine);
    public void RemoveEntity(IMachine machine) => _machines.Remove(machine);

    public void AddEntity(IWorker worker) => _workers.Add(worker);
    public void RemoveEntity(IWorker worker) => _workers.Remove(worker);

    public IReadOnlyList<IAnimal> GetAnimals() => _animals.AsReadOnly();

    public void IncreaseDirtiness(float amount) => Dirtiness = Math.Clamp(Dirtiness + amount, 0f, 100f);
}