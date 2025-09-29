using Farm.Products;

namespace Farm.Animals;

public class Sheep() : Animals.Animal(SheepConfig)
{
    private static readonly AnimalConfig SheepConfig = new AnimalConfig
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
        Console.WriteLine($"{SheepConfig.Name} прыгает по полю.");
        SheepConfig.Productivity += 2;
    }
}
