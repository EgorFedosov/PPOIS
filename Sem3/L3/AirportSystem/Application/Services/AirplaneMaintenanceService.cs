using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Services;

public class AirplaneMaintenanceService : IAirplaneMaintenanceService
{
    public MaintenanceRecord LogMaintenanceRecord(
        IAirplane airplane,
        IMaintenanceTechnician technician,
        string description,
        Money cost)
    {
        ArgumentNullException.ThrowIfNull(airplane);
        ArgumentNullException.ThrowIfNull(technician);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(cost);

        var record = new MaintenanceRecord(DateTime.UtcNow, description, cost, technician.StaffId);

        airplane.AddMaintenanceRecord(record);

        return record;
    }
}