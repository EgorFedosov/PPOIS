using Farm.Products;

namespace Farm.Animal;

public class Goat(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, GoatConfig)
{
    private static readonly AnimalConfig GoatConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 7,
        MaxFoodIntake = 25,
        DirtinessPerToilet = 1.5f,
        Sound = "Baa"
    };
}