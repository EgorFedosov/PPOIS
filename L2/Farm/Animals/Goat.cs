using Farm.Products;

namespace Farm.Animals;

public class Goat(AnimalConfig? config = null) : Animal(config ?? DefaultConfig)
{
    private static readonly AnimalConfig DefaultConfig = new AnimalConfig
    {
        Name = "Goat",
        Sound = "Bleat",
        Product = new Milk(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 10,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 3,
        MaxProductivity = 12,
        ProductivityYoung = 5,
        ProductivityMiddle = 10,
        ProductivityOld = 3,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 8
    };

    public void HeadButtFence()
    {
        Console.WriteLine($"{DefaultConfig.Name} боднула забор.");
        DefaultConfig.Health -= 2; 
    }

}
