using AirportSystem.Domain.Aggregates;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Infrastructure.Repositories;

public class InMemoryAirportCompanyRepository : IAirportCompanyRepository
{
    private readonly IAirportCompany _company = new AirportCompany(new Money(1000000, Currency.Eur));

    public IAirportCompany Get()
    {
        return _company;
    }
}