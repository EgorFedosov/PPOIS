using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities.Persons;

public class MaintenanceTechnician(
    string name,
    int age,
    Gender gender,
    Money money,
    MaintenanceCertification certification,
    int yearsOfExperience)
    : Person(name, age, gender, money), IMaintenanceTechnician
{
    public Guid StaffId { get; } = Guid.NewGuid();
    public StaffDepartment Department { get; } = StaffDepartment.Maintenance;
    public MaintenanceCertification Certification { get; } = certification;
    public int YearsOfExperience { get; } = yearsOfExperience;
}