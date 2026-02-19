namespace AirportSystem.Domain.Enums;

/// <summary>
/// Статус билета пассажира.
/// </summary>
public enum TicketStatus
{
    /// <summary>
    /// Билет оплачен
    /// </summary>
    Paid,

    /// <summary>
    /// Билет аннулирован (пассажиром или компанией).
    /// </summary>
    Cancelled,

    /// <summary>
    /// Рейс по этому билету завершен.
    /// </summary>
    Used
}