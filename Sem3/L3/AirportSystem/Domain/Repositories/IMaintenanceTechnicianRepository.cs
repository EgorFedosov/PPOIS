using AirportSystem.Domain.Interfaces;

namespace AirportSystem.Domain.Repositories;

public interface IMaintenanceTechnicianRepository
{
    IMaintenanceTechnician? GetById(Guid id);
    void Add(IMaintenanceTechnician technician);
}