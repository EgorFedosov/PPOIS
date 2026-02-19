namespace AirportSystem.Domain.Enums;

/// <summary>
/// Текущий статус рейса.
/// </summary>
public enum FlightStatus
{
    Scheduled,
    OnTime,
    Delayed,
    Cancelled,
    InFlight,
    Arrived
}