using Farm.Products;

namespace Farm.Animal;

public class Sheep(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, SheepConfig)
{
    private static readonly AnimalConfig SheepConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 7,
        MaxFoodIntake = 30,
        DirtinessPerToilet = 1.2f,
        Sound = "Baa"
    };
}