using Farm.Configs;
using Farm.Places;

namespace Farm.Interfaces
{
    /// <summary>
    ///     Интерфейс для всех работников фермы.
    ///     Определяет основные действия и свойства, связанные с работой, уровнем, перемещением и оплатой.
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        ///     Получает количество выполненной работы.
        /// </summary>
        /// <returns>Количество выполненной работы.</returns>
       public int WorkCount();

        /// <summary>
        ///     Получает имя работника.
        /// </summary>
        /// <returns>Имя работника.</returns>
        public string Name { get; }

        /// <summary>
        ///     Сбрасывает счётчик выполненной работы.
        /// </summary>
        void ResetWorkCount();

        /// <summary>
        ///     Получает текущий уровень работника и  устанавливает уровень работника.
        /// </summary>
        /// <returns>Уровень работника.</returns>
        public EmployeeLevel Level { get; set; }
        
        /// <summary>
        ///     Начинает выполнение работы.
        /// </summary>
        void Work();

        /// <summary>
        ///     Прекращает выполнение работы.
        /// </summary>
        void StopWork();

        /// <summary>
        ///     Перемещает работника в указанное место на ферме.
        ///     Если место совпадает с текущим или <see langword="null"/>, перемещение не выполняется.
        /// </summary>
        /// <param name="newPlace">Новое место для перемещения.</param>
        void MoveTo(Place newPlace);

        /// <summary>
        ///     Начисляет указанную сумму зарплаты работнику.
        ///     Отрицательные значения игнорируются.
        /// </summary>
        /// <param name="amount">Сумма зарплаты.</param>
        void ReceiveSalary(decimal amount);
    }
}