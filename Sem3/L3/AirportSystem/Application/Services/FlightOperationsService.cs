using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Application.Services;

public class FlightOperationsService(IFlightRepository flightRepository) : IFlightOperationsService
{
    public bool DepartFlight(Guid flightId)
    {
        var flight = flightRepository.GetById(flightId);
        if (flight == null || flight.Status != FlightStatus.Scheduled)
        {
            return false;
        }

        var airplane = flight.Airplane;
        if (airplane.Status != AirplaneStatus.Available)
        {
            return false;
        }

        flight.Status = FlightStatus.InFlight;
        airplane.Status = AirplaneStatus.InFlight;
        return true;
    }

    public bool ArriveFlight(Guid flightId)
    {
        var flight = flightRepository.GetById(flightId);
        if (flight == null || flight.Status != FlightStatus.InFlight)
        {
            return false;
        }

        var airplane = flight.Airplane;
        airplane.Status = AirplaneStatus.Available;

        flight.Status = FlightStatus.Arrived;

        foreach (var passenger in flight.Passengers)
        {
            var ticket = passenger.Tickets.FirstOrDefault(t => t.Flight.FlightId == flightId);
            if (ticket != null)
            {
                ticket.Status = TicketStatus.Used;
            }
        }

        return true;
    }
}