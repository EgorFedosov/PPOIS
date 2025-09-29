using Farm.Products;

namespace Farm.Animals;

public class Duck() : Animals.Animal(DuckConfig)
{
    private static readonly AnimalConfig DuckConfig = new AnimalConfig
    {
        Name = "Duck",
        Sound = "Quack",
        Product = new Egg(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 5,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 1,
        MaxProductivity = 8,
        ProductivityYoung = 3,
        ProductivityMiddle = 6,
        ProductivityOld = 1,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 1,
        AdultAgeLimit = 2,
        OldAgeLimit = 4
    };

    protected override void PerformSpecialAction()
    {
        Console.WriteLine($"{DuckConfig.Name} поплавала в пруду.");
        DuckConfig.Health += 3; 
    }

}
