using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities;

public class Ticket(IPassenger passenger, IFlight flight, TicketStatus status, Money money)
    : ITicket
{
    public IPassenger Passenger { get; } = passenger;
    public IFlight Flight { get; } = flight;
    private readonly IAirplane _airplane = flight.Airplane;

    public bool IsBaggageAllowed(Baggage baggage)
        => _airplane.MaxWeightBaggage > baggage.WeightKg;

    public TicketStatus Status { get; set; } = status;
    public Money Money { get; } = money;
    public string? SeatNumber { get; set; }
    public string BookingReference { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
}