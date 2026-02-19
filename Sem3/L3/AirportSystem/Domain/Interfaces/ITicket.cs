using AirportSystem.Domain.Entities;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Interfaces;

public interface ITicket
{
    IPassenger Passenger { get; }
    IFlight Flight { get; }
    bool IsBaggageAllowed(Baggage baggage);
    TicketStatus Status { get; set; }
    Money Money { get; }
}