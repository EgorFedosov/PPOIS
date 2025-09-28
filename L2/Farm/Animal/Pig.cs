using Farm.Products;

namespace Farm.Animal;

public class Pig() : Animal(PigConfig)
{
    private static readonly AnimalConfig PigConfig = new AnimalConfig
    {
        Name = "Pig",
        Sound = "Oink",
        MaxFoodIntake = 7,
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
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 8
    };
}
