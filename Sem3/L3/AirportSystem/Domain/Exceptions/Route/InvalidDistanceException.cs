namespace AirportSystem.Domain.Exceptions.Route
{
    /// <summary>Расстояние маршрута некорректное.</summary>
    public sealed class InvalidDistanceException(double distance)
        : Exception($"Расстояние должно быть положительным. Указано: {distance} км");
}