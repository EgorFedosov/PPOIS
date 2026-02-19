using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities.Airplanes;

public class Airplane(
    string model,
    uint capacity,
    uint maxWeightBaggage,
    Money price,
    AirplaneStatus status = AirplaneStatus.Available)
    : IAirplane
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Model { get; } = model;
    public uint Capacity { get; } = capacity;
    public Money Price { get; } = price;
    public AirplaneStatus? Status { get; set; } = status;
    public uint MaxWeightBaggage { get; } = maxWeightBaggage;
    private readonly List<MaintenanceRecord> _maintenanceHistory = [];
    public IReadOnlyCollection<MaintenanceRecord> MaintenanceHistory => _maintenanceHistory.AsReadOnly();
    public AirplaneSpecs? Specs { get; set; }
    public DateTime DateOfManufacture { get; set; }

    public void AddMaintenanceRecord(MaintenanceRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        _maintenanceHistory.Add(record);
    }

    public void Print()
    {
        Console.WriteLine($"Airplane Model: {Model} ");
        Console.Write($"Available places: {Capacity}");
        Console.Write($"Price: {Price}, Status: {Status}");
    }
}