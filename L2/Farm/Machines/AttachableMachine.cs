using Farm.Interfaces;
using Farm.Machines.SelfPropelled;

namespace Farm.Machines;

public abstract class AttachableMachine : Machine, IAttachableMachine
{
    private Tractor? Tractor { get; set; }
    private bool IsAttached => Tractor != null;

    public string GetStatus()
    {
        return IsAttached ? $"Подключён к трактору {Tractor?.Name}" : "Не подключён";
    }

    public void SetTractor(Tractor? tractor)
    {
        Tractor = tractor;
    }
}