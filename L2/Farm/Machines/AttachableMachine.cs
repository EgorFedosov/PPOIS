using Farm.Interfaces;
using Farm.Machines.SelfPropelled;

namespace Farm.Machines;

public abstract class AttachableMachine(string name) : Machine(name), IAttachableMachine
{
    private Tractor? Tractor { get; set; }
    private bool IsAttached => Tractor != null;

    public void SetTractor(Tractor? tractor) =>
        Tractor = tractor;


    public string GetStatus()
        => IsAttached ? $"Подключён к трактору {Tractor?.Name}" : "Не подключён";
}