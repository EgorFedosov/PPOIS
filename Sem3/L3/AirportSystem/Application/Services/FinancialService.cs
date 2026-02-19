using AirportSystem.Application.Interfaces;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Application.Services;

public class FinancialService(
    IAirportCompanyRepository companyRepository)
    : IFinancialService
{
    public bool ProcessPayment(IPassenger passenger, Money amount)
    {
        ArgumentNullException.ThrowIfNull(passenger);
        ArgumentNullException.ThrowIfNull(amount);

        var company = companyRepository.Get();
        if (!passenger.Pay(amount)) return false;

        company.AddBalance(amount);
        return true;
    }

    public bool ProcessRefund(IPassenger passenger, Money amount)
    {
        ArgumentNullException.ThrowIfNull(passenger);
        ArgumentNullException.ThrowIfNull(amount);

        var company = companyRepository.Get();

        try
        {
            company.SubtractBalance(amount);
            passenger.Money = passenger.Money.Add(amount);
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}