namespace AirportSystem.Domain.Enums;

/// <summary>
/// Текущий статус самолёта.
/// </summary>
public enum AirplaneStatus
{
    /// <summary>
    /// Готов к рейсу.
    /// </summary>
    Available,

    /// <summary>
    /// Находится на техническом обслуживании, не может быть назначен на рейс.
    /// </summary>
    InMaintenance,

    /// <summary>
    /// Выполняет рейс в данный момент.
    /// </summary>
    InFlight
}