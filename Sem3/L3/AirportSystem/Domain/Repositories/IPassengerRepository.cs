using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IPassengerRepository
{
    IPassenger? GetById(Guid id);
    void Add(IPassenger passenger);
}