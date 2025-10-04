using Farm.Configs;
using Farm.Products;

namespace Farm.Animals;

public class Chicken(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new()
    {
        Name = "Chicken",
        Sound = "Cluck",
        Product = new Egg(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 5,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 1,
        MaxProductivity = 10,
        ProductivityYoung = 5,
        ProductivityMiddle = 8,
        ProductivityOld = 2,
        MinHealth = 0,
        MaxHealth = 100,
        Age = 2,
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 5
    };

    public void DigForWorms()
    {
        Console.WriteLine($"{DefaultConfig.Name} копается в земле и ищет червяков!");
        Eat(5);
    }
}