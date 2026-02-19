namespace AirportSystem.Domain.Exceptions.Country
{
    /// <summary>Пустое или некорректное название страны/города.</summary>
    public sealed class InvalidCountryNameException(string name)
        : Exception($"Название страны/города некорректно или пустое: '{name}'");
}