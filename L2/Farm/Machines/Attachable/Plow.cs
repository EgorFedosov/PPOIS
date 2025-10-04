using Farm.Interfaces;
using Farm.Machines.SelfPropelled;

namespace Farm.Machines.Attachable;

public class Plow(string name) : Machine(name), IAttachableMachine
{
    private Tractor? _tractor;

    public void PlowField()
    {
        if (_tractor == null)
        {
            Console.WriteLine($"{Name}: невозможно вспахать поле — плуг не прицеплён к трактору.");
            return;
        }

        Console.WriteLine($"{Name}: поле вспахано трактором {_tractor.Name}.");
    }

    public void SetTractor(Tractor? tractor)
    {
        _tractor = tractor;
        Console.WriteLine(tractor != null
            ? $"{Name}: прицеплён к трактору {tractor.Name}."
            : $"{Name}: отцеплён от трактора.");
    }
}