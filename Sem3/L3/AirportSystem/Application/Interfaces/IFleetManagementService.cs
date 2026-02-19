using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Interfaces;

public interface IFleetManagementService
{
    void ScheduleMaintenance(IAirplane airplane);

    void CompleteMaintenance(IAirplane airplane);
    IAirplane PurchaseAirplaneWithSpecs(
        string model,
        uint capacity,
        uint maxWeightBaggage,
        Money price,
        AirplaneSpecs specs);
}