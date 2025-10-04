using Farm.Machines.SelfPropelled;

namespace Farm.Interfaces
{
    /// <summary>
    ///     Интерфейс для устройств, которые могут быть прицеплены к трактору.
    /// </summary>
    public interface IAttachableMachine
    {
        /// <summary>
        ///     Устанавливает трактор для этой машины.
        ///     Передача <see langword="null"/> означает отсоединение от трактора.
        /// </summary>
        /// <param name="tractor">Трактор, к которому нужно прицепить устройство, или <see langword="null"/> для отсоединения.</param>
        void SetTractor(Tractor? tractor);
    }
}