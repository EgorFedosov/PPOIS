using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryMaintenanceTechnicianRepository : IMaintenanceTechnicianRepository
{
    private readonly List<IMaintenanceTechnician> _technicians = [];
    public IMaintenanceTechnician? GetById(Guid id) => _technicians.FirstOrDefault(t => t.Id == id);
    public void Add(IMaintenanceTechnician technician) => _technicians.Add(technician);
}