using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.ValueObjects;

namespace AirportSystem.Domain.Entities.Persons;

public class Person(string name, int age, Gender gender, Money money)
    : IPerson
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public int Age { get; } = age;
    public Gender Gender { get; } = gender;
    public Money Money { get; set; } = money;
    public DateTime DateOfBirth { get; set; }
    public Address? Address { get; set; }
    public ContactDetails? ContactDetails { get; set; }
    public Passport?
        Passport { get; set; }

    protected Currency? Currency => Money.Currency;
}