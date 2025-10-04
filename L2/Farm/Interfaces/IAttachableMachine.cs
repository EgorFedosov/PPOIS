using Farm.Machines.SelfPropelled;

namespace Farm.Interfaces;

public interface IAttachableMachine
{
    public void SetTractor(Tractor? tractor);
}