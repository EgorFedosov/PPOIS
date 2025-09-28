using Farm.Products;

namespace Farm.Animal;

public class Rabbit() : Animal(RabbitConfig)
{
    private static readonly AnimalConfig RabbitConfig = new AnimalConfig
    {
        Name = "Rabbit",
        Sound = "Squeak",
        MaxFoodIntake = 1,
        DirtinessPerToilet = 2,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 1,
        MaxProductivity = 5,
        ProductivityYoung = 2,
        ProductivityMiddle = 4,
        ProductivityOld = 1,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 0,
        AdultAgeLimit = 2,
        OldAgeLimit = 4
    };
}