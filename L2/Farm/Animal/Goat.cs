using Farm.Products;

namespace Farm.Animal;

public class Goat() : Animal(GoatConfig)
{
    private static readonly AnimalConfig GoatConfig = new AnimalConfig
    {
        Name = "Goat",
        Sound = "Bleat",
        Product = new Milk(),
        MaxFoodIntake = 4,
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
}
