using Farm.Places;

namespace Farm.Interfaces
{
    /// <summary>
    ///     Интерфейс для всех машин на ферме.
    ///     Определяет основные свойства и действия, доступные для машин.
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        ///     Водитель машины. Может быть <see langword="null"/>, если водитель не назначен.
        /// </summary>
        IWorker? Driver { get; }

        /// <summary>
        ///     Название машины.
        /// </summary>
        string? Name { get; }

        /// <summary>
        ///     Текущее местоположение машины на ферме.
        /// </summary>
        Place? Location { get; }

        /// <summary>
        ///     Состояние машины: включена (<c>true</c>) или выключена (<c>false</c>).
        /// </summary>
        bool IsOn { get; }

        /// <summary>
        ///     Перемещает машину в указанное место на ферме.
        /// </summary>
        /// <param name="newPlace">Новое место для машины.</param>
        void MoveTo(Place newPlace);

        /// <summary>
        ///     Назначает водителя для машины. 
        ///     Передача <see langword="null"/> снимает водителя.
        /// </summary>
        /// <param name="driver">Водитель машины или <see langword="null"/> для снятия.</param>
        void AssignDriver(IWorker? driver);

        /// <summary>
        ///     Перемещает машину к указанной точке назначения.
        ///     Требует наличия водителя и включённого состояния машины.
        /// </summary>
        /// <param name="destination">Место назначения.</param>
        void DriveTo(Place destination);

        /// <summary>
        ///     Выключает машину.
        /// </summary>
        void TurnOff();
        /// <summary>
        ///     Включает машину.
        /// </summary>
        void TurnOn();
    }
}