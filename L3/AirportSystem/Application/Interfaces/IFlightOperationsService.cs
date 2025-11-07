namespace AirportSystem.Application.Interfaces;

public interface IFlightOperationsService
{
    bool DepartFlight(Guid flightId);
    bool ArriveFlight(Guid flightId);
}