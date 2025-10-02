using Farm.Products;

namespace Farm.Animals;

public class Rabbit(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new AnimalConfig
    {
        Name = "Rabbit",
        Sound = "Squeak",
        Product = new Meat(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 2,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 1,
        MaxProductivity = 5,
        ProductivityYoung = 2,
        ProductivityMiddle = 4,
        ProductivityOld = 1,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 0,
        AdultAgeLimit = 2,
        OldAgeLimit = 4
    };

    public void DigBurrow()
    {
        Console.WriteLine($"{DefaultConfig.Name} роет нору.");
        DefaultConfig.Hunger -= 1;
        GoToToilet();
    }
}