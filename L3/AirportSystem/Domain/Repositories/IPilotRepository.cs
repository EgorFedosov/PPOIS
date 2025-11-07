using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IPilotRepository
{
    IPilot? GetById(Guid id);
    void Add(IPilot pilot);
}