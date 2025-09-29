using Farm.Products;

namespace Farm.Animals;

public class Pig() : Animals.Animal(PigConfig)
{
    private static readonly AnimalConfig PigConfig = new AnimalConfig
    {
        Name = "Pig",
        Sound = "Oink",
        Product = new Meat(),
        MaxFoodIntake = 100,
        DirtinessPerToilet = 15,
        MinHungry = 0,
        MaxHungry = 100,
        MinProductivity = 5,
        MaxProductivity = 20,
        ProductivityYoung = 10,
        ProductivityMiddle = 15,
        ProductivityOld = 5,
        MinHealth = 0,
        MaxHealth = 100,
        YoungAgeLimit = 1,
        AdultAgeLimit = 3,
        OldAgeLimit = 8
    };

    protected override void PerformSpecialAction()
    {
        Console.WriteLine($"{PigConfig.Name} роется в грязи — счастье +10 :)");
        PigConfig.Health += 5;
        GoToToilet(); 
    }

}
