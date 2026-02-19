using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryPilotRepository : IPilotRepository
{
    private readonly List<IPilot> _pilots = [];
    public IPilot? GetById(Guid id) => _pilots.FirstOrDefault(p => p.Id == id);
    public void Add(IPilot pilot) => _pilots.Add(pilot);
}