using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryFlightRepository : IFlightRepository
{
    private readonly List<IFlight> _flights = [];

    public IFlight?
        GetById(Guid id)
    {
        return _flights.FirstOrDefault(f => f.FlightId == id);
    }

    public IEnumerable<IFlight> GetAll()
    {
        return _flights;
    }

    public void Add(IFlight flight)
    {
        _flights.Add(flight);
    }
}