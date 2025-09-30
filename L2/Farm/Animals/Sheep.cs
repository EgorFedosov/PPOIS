using Farm.Products;

namespace Farm.Animals;

public class Sheep(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new AnimalConfig
    {
        Name = "Sheep",
        Sound = "Baa",
        Product = new Wool(),
        DirtinessPerToilet = 10,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 5,
        MaxProductivity = 15,
        ProductivityYoung = 7,
        ProductivityMiddle = 12,
        ProductivityOld = 5,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 1,
        AdultAgeLimit = 4,
        OldAgeLimit = 10
    };

    protected override void PerformSpecialAction()
    {
        Console.WriteLine($"{DefaultConfig.Name} прыгает по полю.");
        DefaultConfig.Productivity += 2;
    }
}
