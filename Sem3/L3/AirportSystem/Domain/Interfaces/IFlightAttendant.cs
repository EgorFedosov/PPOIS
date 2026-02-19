using AirportSystem.Domain.Enums;

namespace AirportSystem.Domain.Interfaces;

public interface IFlightAttendant : IPerson
{
    Guid StaffId { get; }
    StaffDepartment Department { get; }
    IReadOnlyCollection<string> ServiceLanguages { get; }
    DateTime CertificationExpiry { get; }
}