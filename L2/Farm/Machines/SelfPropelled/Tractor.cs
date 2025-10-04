using Farm.Interfaces;

namespace Farm.Machines.SelfPropelled;

public class Tractor(string name) : Machine(name)
{
    private readonly List<IAttachableMachine> _attachments = [];
    public IReadOnlyList<IAttachableMachine> Attachments => _attachments.AsReadOnly();

    public void Attach(IAttachableMachine machine)
    {
        if (!_attachments.Contains(machine))
        {
            _attachments.Add(machine);
            machine.SetTractor(this);
        } // TODO else exception
    }

    public void Detach(IAttachableMachine machine)
    {
        if (_attachments.Contains(machine))
        {
            _attachments.Remove(machine);
            machine.SetTractor(null);
        } // TODO else exception
    }
}