using AirportSystem.Domain.Entities;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Interfaces;

public interface IPassenger : IPerson
{
    List<ITicket> Tickets { get; }

    Baggage?
        Baggage { get; }

    public bool AddTicket(ITicket ticket);
    public void RemoveTicket(ITicket ticket);
    public void AssignBaggage(Baggage baggage);
    bool Pay(Money money);
}