using Farm.Products;

namespace Farm.Animal;

public class Rabbit(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, RabbitConfig)
{
    private static readonly AnimalConfig RabbitConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 5,
        MaxFoodIntake = 5,
        DirtinessPerToilet = 0.2f,
        Sound = "Squeak"
    };
}