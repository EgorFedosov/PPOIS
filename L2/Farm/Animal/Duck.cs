using Farm.Products;

namespace Farm.Animal;

public class Duck : Animal
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

    public Duck(string name, int age, Place.Place place, Product product)
        : base(name, age, place, product, DuckConfig) { }
}