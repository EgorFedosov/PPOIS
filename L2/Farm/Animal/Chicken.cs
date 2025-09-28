using Farm.Products;

namespace Farm.Animal;

public class Chicken() : Animal(ChickenConfig)
{
    private static readonly AnimalConfig ChickenConfig = new AnimalConfig
    {
        Name = "Chicken",
        Sound = "Cluck",
        Product = new Egg(),
        MaxFoodIntake = 1,
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
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 5
    };
}
