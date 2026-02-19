using AirportSystem.Domain.Enums;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Interfaces;

public interface IAirplane
{
    Guid Id { get; }
    string Model { get; }
    uint Capacity { get; }
    uint MaxWeightBaggage { get; }
    Money Price { get; }
    AirplaneStatus? Status { get; set; }

    IReadOnlyCollection<MaintenanceRecord> MaintenanceHistory { get; }
    AirplaneSpecs? Specs { get; set; }
    DateTime DateOfManufacture { get; set; }

    void Print();

    void AddMaintenanceRecord(MaintenanceRecord record);
}