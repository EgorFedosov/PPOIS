using Farm.Interface;

namespace Farm.Place;

public abstract class Place
{
    protected string? PlaceName { get; set; }
    protected uint PlaceId { get; set; }
    protected uint Area { get; set; }
    private float Dirtiness { get; set; }
    private readonly List<IPlaceable> _entities = new();

    public void AddEntity(IPlaceable entity) => _entities.Add(entity);
    public void RemoveEntity(IPlaceable entity) => _entities.Remove(entity);
    public IReadOnlyList<IPlaceable> GetEntities() => _entities.AsReadOnly();

    public void IncreaseDirtiness(float amount)
    {
        Dirtiness = Math.Clamp(Dirtiness + amount, 0f, 100f);
    }

    public void Clean()
    {
        Dirtiness = 0;
    }
}