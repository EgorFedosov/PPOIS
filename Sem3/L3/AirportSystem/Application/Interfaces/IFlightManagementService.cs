using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Interfaces;

public interface IFlightManagementService
{
    IFlight CreateFlight(
        IAirplane airplane,
        List<IPilot> crew,
        Route route,
        Money ticketPrice);

    void UpdateTicketPrice(IFlight flight, Money newPrice);
}