using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Interfaces;

public interface IAirportCompany
{
    Money Balance { get; }
    void AddBalance(Money amount);
    void SubtractBalance(Money amount);
    string RegistrationNumber { get; set; }
    string TaxId { get; set; }

    Address? CompanyAddress { get; set; }
}