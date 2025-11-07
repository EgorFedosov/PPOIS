using AirportSystem.Domain.Exceptions.Route;

namespace AirportSystem.Domain.ValueObjects
{
    public sealed class Route : IEquatable<Route>
    {
        private Country Destination { get; }
        private double DistanceKm { get; }

        public Route(Country destination, double distanceKm)
        {
            if (distanceKm <= 0)
                throw new InvalidDistanceException(distanceKm);

            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            DistanceKm = distanceKm;
        }

        public void Print()
        {
            Console.WriteLine($"{Destination.Name} - {DistanceKm}");
        }

        public bool Equals(Route? other)
            => other is not null && Destination.Equals(other.Destination);

        public override bool Equals(object? obj)
            => obj is Route route && Equals(route);

        public override int GetHashCode()
            => Destination.GetHashCode();

        public static bool operator ==(Route? left, Route? right)
            => Equals(left, right);

        public static bool operator !=(Route? left, Route? right)
            => !Equals(left, right);
    }
}