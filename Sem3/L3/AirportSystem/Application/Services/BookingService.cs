using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Entities;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Repositories;

namespace AirportSystem.Application.Services;

public class BookingService(
    IFlightRepository flightRepository,
    IPassengerRepository passengerRepository,
    IFinancialService financialService)
    : IBookingService
{
    public void PrintAllFlights()
    {
        var flights = flightRepository.GetAll();
        foreach (var flight in flights)
        {
            flight.Print();
        }
    }

    public bool BuyTicket(Guid passengerId, Guid flightId)
    {
        var passenger = passengerRepository.GetById(passengerId);
        var flight = flightRepository.GetById(flightId);

        if (passenger == null || flight == null)
            return false;

        if (flight.Passengers.Count >= flight.Airplane.Capacity)
            return false;

        var ticket = new Ticket(passenger, flight, TicketStatus.Paid, flight.TicketPrice);

        if (passenger.Baggage != null && !ticket.IsBaggageAllowed(passenger.Baggage))
        {
            return false;
        }

        if (!financialService.ProcessPayment(passenger, ticket.Money))
        {
            return false;
        }

        if (!passenger.AddTicket(ticket))
        {
            financialService.ProcessRefund(passenger, ticket.Money);
            return false;
        }

        if (!flight.AddPassenger(passenger))
        {
            financialService.ProcessRefund(passenger, ticket.Money);
            passenger.RemoveTicket(ticket);
            return false;
        }

        return true;
    }
}
