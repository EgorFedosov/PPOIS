namespace AirportSystem.Domain.ValueObjects;

/// <summary>
/// Контактная информация.
/// </summary>
public sealed class ContactDetails(string? email, string? phoneNumber) : IEquatable<ContactDetails>
{
    public string Email { get; } = email ?? string.Empty;
    public string PhoneNumber { get; } = phoneNumber ?? string.Empty;

    public bool Equals(ContactDetails? other)
    {
        if (other is null) return false;
        return Email == other.Email && PhoneNumber == other.PhoneNumber;
    }

    public override bool Equals(object? obj) => obj is ContactDetails other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Email, PhoneNumber);
}