using Farm.Products;

namespace Farm.Animal;

public class Pig : Animal
{
    private static readonly AnimalConfig PigConfig = new AnimalConfig
    {
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 6,
        MaxFoodIntake = 40,
        DirtinessPerToilet = 3f,
        Sound = "Oink"
    };

    public Pig(string name, int age, Place.Place place, Product product)
        : base(name, age, place, product, PigConfig) { }
}