using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;
using AirportSystem.Domain.Enums;

namespace AirportSystem.Domain.Aggregates;

public class Flight(IAirplane airplane, List<IPilot>? crew, Route route, Money ticketPrice) : IFlight
{
    private readonly List<IPilot> _crew = crew ??
                                          [];

    private readonly List<IPassenger> _passengers = [];

    public IAirplane Airplane { get; } = airplane;
    public IReadOnlyCollection<IPilot> Crew => _crew.AsReadOnly();
    public IReadOnlyCollection<IPassenger> Passengers => _passengers.AsReadOnly();
    public Route Route { get; } = route;
    public Money TicketPrice { get; private set; } = ticketPrice;
    public Guid FlightId { get; } = Guid.NewGuid();
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;
    public DateTime ScheduledDeparture { get; set; }
    public DateTime ScheduledArrival { get; set; }
    public string? Terminal { get; set; }
    public string? Gate { get; set; }


    public void Print()
    {
        Airplane.Print();
        TicketPrice.Print();
        Console.WriteLine($"Guid: {FlightId}");
    }

    public bool AddPassenger(IPassenger passenger)
    {
        if (_passengers.Count >= Airplane.Capacity) return false;
        _passengers.Add(passenger);
        return true;
    }

    public void UpdateTicketPrice(Money newPrice)
    {
        if (newPrice == TicketPrice) return;
        TicketPrice = newPrice;
    }
}