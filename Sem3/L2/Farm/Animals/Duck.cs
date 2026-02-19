using Farm.Configs;
using Farm.Products;

namespace Farm.Animals;

public class Duck(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new()
    {
        Name = "Duck",
        Sound = "Quack",
        Product = new Egg(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 5,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 1,
        MaxProductivity = 8,
        ProductivityYoung = 3,
        ProductivityMiddle = 6,
        ProductivityOld = 1,
        MinHealth = 0,
        MaxHealth = 100,
        Age = 1,
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 4
    };

    public void Swim()
    {
        Console.WriteLine($"{DefaultConfig.Name} поплавала в пруду.");
        DefaultConfig.Health += 3;
    }
}