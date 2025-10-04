namespace Farm.Machines.Attachable;

public class CropSprayer : AttachableMachine
{
    public void SprayField()
    {
        Console.WriteLine($"{Name}: поле обработано опрыскивателем.");
    }
}