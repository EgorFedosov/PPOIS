using Farm.Interfaces;
using Farm.Machines.SelfPropelled;

namespace Farm.Machines.Attachable;

public class CropSprayer(string? name) : Machine(name ?? "CropSprayer"), IAttachableMachine
{
    private Tractor? _tractor;

    public void SprayField()
    {
        if (_tractor == null)
        {
            Console.WriteLine($"{Name}: невозможно обработать поле — опрыскиватель не прицеплён к трактору.");
            return;
        }

        Console.WriteLine($"{Name}: поле обработано опрыскивателем, работающим с трактором {_tractor.Name}.");
    }

    public void SetTractor(Tractor? tractor)
    {
        _tractor = tractor;

        Console.WriteLine(tractor != null
            ? $"{Name}: прицеплён к трактору {tractor.Name}."
            : $"{Name}: отцеплён от трактора.");
    }
}