using AirportSystem.Domain.Enums;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Interfaces;

public interface IPerson
{
    Guid Id { get; }
    string Name { get; }
    int Age { get; }
    Gender Gender { get; }
    Money Money { get; set; }
}