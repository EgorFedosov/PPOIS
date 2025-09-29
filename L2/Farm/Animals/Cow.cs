using Farm.Products;

namespace Farm.Animals;

public class Cow() : Animals.Animal(CowConfig)
{
    private static readonly AnimalConfig CowConfig = new AnimalConfig
    {
        Name = "Cow",
        Sound = "Moo",
        Product = new Milk(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 20,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 10,
        MaxProductivity = 30,
        ProductivityYoung = 15,
        ProductivityMiddle = 25,
        ProductivityOld = 10,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 2,
        AdultAgeLimit = 5,
        OldAgeLimit = 12
    };

    protected override void PerformSpecialAction()
    {
        Console.WriteLine($"{CowConfig.Name} отдыхает на лужайке.");
        CowConfig.Health += 5;
    }
}