namespace AirportSystem.Domain.ValueObjects;

/// <summary>
/// Физический адрес.
/// </summary>
public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }
    public Country Country { get; }

    public Address(string street, string city, string zipCode, Country country)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street cannot be empty", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentException("ZipCode cannot be empty", nameof(zipCode));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street && City == other.City && ZipCode == other.ZipCode &&
               Country.Equals(other.Country);
    }

    public override bool Equals(object? obj) => obj is Address other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Street, City, ZipCode, Country);
}