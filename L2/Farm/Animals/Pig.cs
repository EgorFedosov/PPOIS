using Farm.Configs;
using Farm.Products;

namespace Farm.Animals;

public class Pig(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new()
    {
        Name = "Pig",
        Sound = "Oink",
        Product = new Meat(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 15,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 5,
        MaxProductivity = 20,
        ProductivityYoung = 10,
        ProductivityMiddle = 15,
        ProductivityOld = 5,
        MinHealth = 0,
        MaxHealth = 100,
        Age = 2,
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 8
    };

    public void RollInMud()
    {
        Console.WriteLine($"{DefaultConfig.Name} роется в грязи — счастье +10 :)");
        DefaultConfig.Health += 5;
        GoToToilet();
    }
}