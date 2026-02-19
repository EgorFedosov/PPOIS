using Farm.Interfaces;
using Farm.Exceptions;
using Farm.Places;

namespace Farm.Machines.SelfPropelled;

public class Tractor(string? name) : Machine(name ?? "Tractor")
{
    private readonly List<IAttachableMachine> _attachments = [];
    public IReadOnlyList<IAttachableMachine> Attachments => _attachments.AsReadOnly();

    public override void MoveTo(Place newPlace)
    {
        base.MoveTo(newPlace);
        foreach (var attachment in _attachments)
        {
            if (attachment is Machine machine)
            {
                machine.MoveTo(newPlace);
            }
        }
    }

    public void Attach(IAttachableMachine machine)
    {
        if (!_attachments.Contains(machine))
        {
            _attachments.Add(machine);
            machine.SetTractor(this);
        }
        else
        {
            throw new AttachmentAlreadyConnectedException(
                "Попытка присоединить уже присоединённое навесное оборудование");
        }
    }

    public void Detach(IAttachableMachine machine)
    {
        if (_attachments.Contains(machine))
        {
            _attachments.Remove(machine);
            machine.SetTractor(null);
        }
        else
        {
            throw new AttachmentNotConnectedException("Попытка отсоединить не присоединённое навесное оборудование");
        }
    }
}