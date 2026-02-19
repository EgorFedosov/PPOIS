namespace AirportSystem.Application.Interfaces;

public interface IBookingService
{
    void PrintAllFlights();
    bool BuyTicket(Guid passengerId, Guid flightId);
}