using Farm.Places;
using Farm.Products;
using Farm.Configs;

namespace Farm.Configs;

public class AnimalConfig
{
    private const int DefaultHunger = 70;
    private const int DefaultHealth = 90;
    private const int DefaultProductivityYoung = 30;
    private const int DefaultProductivityMiddle = 70;
    private const int DefaultProductivityOld = 20;
    private const int DefaultMaxFoodIntake = 100;
    private const int DefaultLowHungerThreshold1 = 30;
    private const int DefaultLowHungerThreshold2 = 20;
    private const int DefaultLowHungerThreshold3 = 10;
    private const int DefaultProductivityPenalty1 = 1;
    private const int DefaultProductivityPenalty2 = 3;
    private const int DefaultProductivityPenalty3 = 7;

    private int _hunger = DefaultHunger;
    private int _health = DefaultHealth;
    private int _productivity;

    public int Hunger
    {
        get => _hunger;
        set => _hunger = Math.Clamp(value, MinHungry, MaxHungry);
    }

    public int Productivity
    {
        get => _productivity;
        set => _productivity = Math.Clamp(value, MinProductivity, MaxProductivity);
    }

    public int Health
    {
        get => _health;
        set => _health = Math.Clamp(value, MinHealth, MaxHealth);
    }

    public Product? Product { get; init; }
    public Place? Place { get; set; }
    public string? Name { get; init; }
    public int Age { get; set; }

    public string? Sound { get; init; }
    public int MaxFoodIntake { get; init; } = DefaultMaxFoodIntake;
    public float DirtinessPerToilet { get; init; }

    public int MinHungry { get; init; }
    public int MaxHungry { get; init; }

    public int MinProductivity { get; init; }
    public int MaxProductivity { get; init; }

    public int ProductivityYoung { get; init; } = DefaultProductivityYoung;
    public int ProductivityMiddle { get; init; } = DefaultProductivityMiddle;
    public int ProductivityOld { get; init; } = DefaultProductivityOld;


    public int MinHealth { get; init; }
    public int MaxHealth { get; init; }

    public int LowHungerThreshold1 { get; set; } = DefaultLowHungerThreshold1;
    public int LowHungerThreshold2 { get; set; } = DefaultLowHungerThreshold2;
    public int LowHungerThreshold3 { get; set; } = DefaultLowHungerThreshold3;

    public int ProductivityPenalty1 { get; set; } = DefaultProductivityPenalty1;
    public int ProductivityPenalty2 { get; set; } = DefaultProductivityPenalty2;
    public int ProductivityPenalty3 { get; set; } = DefaultProductivityPenalty3;

    public int YoungAgeLimit { get; init; }
    public int AdultAgeLimit { get; init; }
    public int OldAgeLimit { get; init; }
}
