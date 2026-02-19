using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IFlightRepository
{
    IFlight? GetById(Guid id);
    IEnumerable<IFlight> GetAll();
    void Add(IFlight flight);
}