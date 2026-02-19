using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities.Persons;

public class FlightAttendant(
    string name,
    int age,
    Gender gender,
    Money money,
    List<string>? serviceLanguages,
    DateTime certificationExpiry)
    : Person(name, age, gender, money), IFlightAttendant
{
    public Guid StaffId { get; } = Guid.NewGuid();
    public StaffDepartment Department { get; } = StaffDepartment.FlightCrew;
    public IReadOnlyCollection<string> ServiceLanguages { get; } = serviceLanguages?.AsReadOnly() ?? new List<string>().AsReadOnly();
    public DateTime CertificationExpiry { get; } = certificationExpiry;
}