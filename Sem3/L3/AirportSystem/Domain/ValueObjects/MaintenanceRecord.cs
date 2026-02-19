namespace AirportSystem.Domain.ValueObjects;

/// <summary>
/// Запись об обслуживании.
/// </summary>
public sealed class MaintenanceRecord(DateTime datePerformed, string description, Money cost, Guid technicianId)
{
    public Guid MaintenanceId { get; } = Guid.NewGuid();
    public DateTime DatePerformed { get; } = datePerformed;
    public string Description { get; } = description;
    public Money Cost { get; } = cost;
    public Guid TechnicianId { get; } = technicianId;
}