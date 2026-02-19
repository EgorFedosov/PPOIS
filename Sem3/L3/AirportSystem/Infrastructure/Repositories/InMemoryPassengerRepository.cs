using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryPassengerRepository : IPassengerRepository
{
    private readonly List<IPassenger> _passengers = [];
    public IPassenger? GetById(Guid id) => _passengers.FirstOrDefault(p => p.Id == id);
    public void Add(IPassenger passenger) => _passengers.Add(passenger);
}