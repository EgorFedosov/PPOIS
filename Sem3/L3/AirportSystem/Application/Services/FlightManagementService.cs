using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Aggregates;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Services;

public class FlightManagementService(
    IFlightRepository flightRepository)
    : IFlightManagementService
{
    public IFlight CreateFlight(IAirplane airplane, List<IPilot> crew, Route route, Money ticketPrice)
    {
        ArgumentNullException.ThrowIfNull(airplane);
        ArgumentNullException.ThrowIfNull(route);
        ArgumentNullException.ThrowIfNull(ticketPrice);
        if (crew == null || crew.Count == 0)
            throw new ArgumentException("Crew cannot be empty", nameof(crew));

        var flight = new Flight(airplane, crew, route, ticketPrice);
        flightRepository.Add(flight);
        return flight;
    }

    public void UpdateTicketPrice(IFlight flight, Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(flight);
        ArgumentNullException.ThrowIfNull(newPrice);

        flight.UpdateTicketPrice(newPrice);
    }
}