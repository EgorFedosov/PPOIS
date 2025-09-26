using Farm.Products;


namespace Farm.Animal;

public class Cow(string name, int age, Place.Place place, Product product)
    : Animal(name, age, place, product, CowConfig)
{
    private static readonly AnimalConfig CowConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 4,
        OldAgeLimit = 10,
        MaxFoodIntake = 50,
        DirtinessPerToilet = 5f,
        Sound = "Moo"
    };
}