using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Aggregates;

public class AirportCompany(Money initialBalance) : IAirportCompany
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public Address? CompanyAddress { get; set; }
    public Money Balance { get; private set; } = initialBalance;

    public void AddBalance(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);
        Balance += amount;
    }

    public void SubtractBalance(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);
        Balance = Balance.Subtract(amount);
    }
}