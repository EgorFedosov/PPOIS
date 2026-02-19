using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Interfaces;

public interface IFinancialService
{
    bool ProcessPayment(IPassenger passenger, Money amount);
    bool ProcessRefund(IPassenger passenger, Money amount);
}