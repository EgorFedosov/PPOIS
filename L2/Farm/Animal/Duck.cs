using Farm.Products;

namespace Farm.Animal;

public class Duck(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, DuckConfig)
{
    private static readonly AnimalConfig DuckConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 5,
        MaxFoodIntake = 20,
        DirtinessPerToilet = 1f,
        Sound = "Quack"
    };
}