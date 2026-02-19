using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Interfaces;
public interface IHumanResourcesService
{
    IFlightAttendant HireFlightAttendant(
        string name,
        int age,
        Gender gender,
        Money salary,
        List<string> languages,
        DateTime certificationExpiry);
    IMaintenanceTechnician HireMaintenanceTechnician(
        string name,
        int age,
        Gender gender,
        Money salary,
        MaintenanceCertification certification,
        int yearsOfExperience);
    IPilot HirePilot(
        string name,
        int age,
        Gender gender,
        Money salary);
}