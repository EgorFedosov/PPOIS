using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryAirplaneRepository : IAirplaneRepository
{
    private readonly List<IAirplane> _airplanes = [];

    public IAirplane? GetById(Guid id)
    {
        return _airplanes.FirstOrDefault(a => a.Id == id);
    }

    public IEnumerable<IAirplane> GetAll()
    {
        return _airplanes;
    }

    public void Add(IAirplane airplane)
    {
        _airplanes.Add(airplane);
    }
}