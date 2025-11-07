using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryFlightAttendantRepository : IFlightAttendantRepository
{
    private readonly List<IFlightAttendant> _attendants = [];
    public IFlightAttendant? GetById(Guid id) => _attendants.FirstOrDefault(a => a.Id == id);
    public void Add(IFlightAttendant attendant) => _attendants.Add(attendant);
}