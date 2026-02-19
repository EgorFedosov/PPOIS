using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Entities.Persons;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Services;

public class HumanResourcesService(
    IFlightAttendantRepository attendantRepository,
    IMaintenanceTechnicianRepository technicianRepository,
    IPilotRepository pilotRepository)
    : IHumanResourcesService
{
    public IFlightAttendant HireFlightAttendant(
        string name, int age, Gender gender, Money salary,
        List<string> languages, DateTime certificationExpiry)
    {
        var attendant = new FlightAttendant(name, age, gender, salary, languages, certificationExpiry);
        attendantRepository.Add(attendant);
        return attendant;
    }

    public IMaintenanceTechnician HireMaintenanceTechnician(
        string name, int age, Gender gender, Money salary,
        MaintenanceCertification certification, int yearsOfExperience)
    {
        var tech = new MaintenanceTechnician(name, age, gender, salary, certification, yearsOfExperience);
        technicianRepository.Add(tech);
        return tech;
    }

    public IPilot HirePilot(
        string name, int age, Gender gender, Money salary)
    {
        var pilot = new Pilot(name, age, gender, salary);
        pilotRepository.Add(pilot);
        return pilot;
    }
}