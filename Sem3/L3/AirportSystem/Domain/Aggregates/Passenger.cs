using AirportSystem.Domain.Entities;
using AirportSystem.Domain.Entities.Persons;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Aggregates;

public class Passenger(string name, int age, Gender gender, Money money, List<ITicket>? tickets, Baggage? baggage)
    : Person(name, age, gender, money), IPassenger
{
    public List<ITicket> Tickets { get; } = tickets ?? [];
    public Baggage? Baggage { get; private set; } = baggage;

    public bool AddTicket(ITicket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        if (Baggage != null && !ticket.IsBaggageAllowed(Baggage))
        {
            Console.WriteLine("Baggage not allowed");
            return false;
        }

        ticket.Status = TicketStatus.Paid;
        Tickets.Add(ticket);
        return true;
    }

    public void RemoveTicket(ITicket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        Tickets.Remove(ticket);
    }

    public void AssignBaggage(Baggage baggage)
    {
        ArgumentNullException.ThrowIfNull(baggage);
        Baggage = baggage;
    }

    public bool Pay(Money money)
    {
        if (money.Currency != Currency)
            return false;
        if (money.Amount > Money.Amount)
            return false;

        Money = Money.Subtract(money);
        return true;
    }
}