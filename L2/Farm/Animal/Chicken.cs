using Farm.Products;

namespace Farm.Animal;

public class Chicken(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, ChickenConfig)
{
    private static readonly AnimalConfig ChickenConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 4,
        MaxFoodIntake = 15,
        DirtinessPerToilet = 0.5f,
        Sound = "Cluck"
    };
}