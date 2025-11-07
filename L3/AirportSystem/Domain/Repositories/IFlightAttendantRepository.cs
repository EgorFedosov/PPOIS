using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IFlightAttendantRepository
{
    IFlightAttendant? GetById(Guid id);
    void Add(IFlightAttendant attendant);
}