using Farm.Products;

namespace Farm.Animal;

public class Sheep() : Animal(SheepConfig)
{
    private static readonly AnimalConfig SheepConfig = new AnimalConfig
    {
        Name = "Sheep",
        Sound = "Baa",
        Product = new Wool(),
        MaxFoodIntake = 5,
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
}
