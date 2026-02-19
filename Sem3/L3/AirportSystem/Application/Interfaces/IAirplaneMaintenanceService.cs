using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Interfaces;

public interface IAirplaneMaintenanceService
{
    MaintenanceRecord LogMaintenanceRecord(
        IAirplane airplane,
        IMaintenanceTechnician technician,
        string description,
        Money cost);
}