namespace Farm.Machines.Attachable;

public class Seeder : AttachableMachine

{
    public void SeedField()
    {
        Console.WriteLine($"{Name}: поле засеяно.");
    }
}