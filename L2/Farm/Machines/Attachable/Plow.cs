namespace Farm.Machines.Attachable;

public class Plow : AttachableMachine

{
    public void PlowField()
    {
        Console.WriteLine($"{Name}: поле вспахано.");
    }
}