using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Entities.Airplanes;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Services;

public class FleetManagementService(
    IAirplaneRepository airplaneRepository)
    : IFleetManagementService
{
    public void ScheduleMaintenance(IAirplane airplane)
    {
        ArgumentNullException.ThrowIfNull(airplane);
        if (airplane.Status == AirplaneStatus.InFlight)
        {
            throw new InvalidOperationException(
                "Cannot schedule maintenance for an airplane that is currently in flight.");
        }

        airplane.Status = AirplaneStatus.InMaintenance;
    }

    public void CompleteMaintenance(IAirplane airplane)
    {
        ArgumentNullException.ThrowIfNull(airplane);
        if (airplane.Status == AirplaneStatus.InMaintenance)
        {
            airplane.Status = AirplaneStatus.Available;
        }
    }

    public IAirplane PurchaseAirplaneWithSpecs(
        string model,
        uint capacity,
        uint maxWeightBaggage,
        Money price,
        AirplaneSpecs specs)
    {
        ArgumentNullException.ThrowIfNull(price);
        ArgumentNullException.ThrowIfNull(specs);

        var airplane = new Airplane(
            model,
            capacity,
            maxWeightBaggage,
            price)
        {
            Specs = specs,
            DateOfManufacture = DateTime.UtcNow
        };
        airplaneRepository.Add(airplane);

        return airplane;
    }
}