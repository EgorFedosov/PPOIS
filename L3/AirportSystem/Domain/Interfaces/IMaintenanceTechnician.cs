using AirportSystem.Domain.Enums;

namespace AirportSystem.Domain.Interfaces;

public interface IMaintenanceTechnician : IPerson
{
    Guid StaffId { get; }
    StaffDepartment Department { get; }
    MaintenanceCertification Certification { get; }
    int YearsOfExperience { get; }
}