using Farm.Interfaces;
using Farm.Machines.SelfPropelled;

namespace Farm.Machines.Attachable;

public class Seeder(string? name) : Machine(name ?? "Seeder"), IAttachableMachine
{
    private Tractor? _tractor;

    public void SeedField()
    {
        if (_tractor == null)
        {
            Console.WriteLine($"{Name}: невозможно засеять поле — сеялка не прицеплена к трактору.");
            return;
        }

        Console.WriteLine($"{Name}: поле засеяно сеялкой, работающей с трактором {_tractor.Name}.");
    }

    public void SetTractor(Tractor? tractor)
    {
        _tractor = tractor;

        Console.WriteLine(tractor != null
            ? $"{Name}: прицеплена к трактору {tractor.Name}."
            : $"{Name}: отцеплена от трактора.");
    }
}