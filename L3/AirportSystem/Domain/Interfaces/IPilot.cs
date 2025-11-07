namespace AirportSystem.Domain.Interfaces;

public interface IPilot : IPerson
{
    uint FlightHours { get; }
}