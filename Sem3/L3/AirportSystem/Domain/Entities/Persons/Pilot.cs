using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities.Persons;

public class Pilot(string name, int age, Gender gender, Money money, uint flightHours = 0)
    : Person(name, age, gender, money), IPilot
{
    public uint FlightHours { get; private set; } = flightHours;
    public Guid StaffId { get; } = Guid.NewGuid();
    public StaffDepartment Department { get; } = StaffDepartment.FlightCrew;
    public void AddFlightHours(uint hours)
    {
        if (hours == 0) return;
        FlightHours += hours;
    }
}