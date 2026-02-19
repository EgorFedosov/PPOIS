namespace AirportSystem.Domain.ValueObjects;

/// <summary>
/// Паспортные данные.
/// </summary>
public sealed class Passport : IEquatable<Passport>
{
    public string PassportNumber { get; }
    public Country IssueCountry { get; }
    public DateTime ExpiryDate { get; }

    public Passport(string passportNumber, Country issueCountry, DateTime expiryDate)
    {
        if (string.IsNullOrWhiteSpace(passportNumber))
            throw new ArgumentException("Passport number cannot be empty", nameof(passportNumber));

        PassportNumber = passportNumber;
        IssueCountry = issueCountry ?? throw new ArgumentNullException(nameof(issueCountry));
        ExpiryDate = expiryDate;
    }

    public bool IsValid(DateTime atDate) => atDate < ExpiryDate;

    public bool Equals(Passport? other)
    {
        if (other is null) return false;
        return PassportNumber == other.PassportNumber && IssueCountry.Equals(other.IssueCountry);
    }

    public override bool Equals(object? obj) => obj is Passport other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(PassportNumber, IssueCountry);
}