using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IAirplaneRepository
{
    IAirplane? GetById(Guid id);
    IEnumerable<IAirplane> GetAll();
    void Add(IAirplane airplane);
}