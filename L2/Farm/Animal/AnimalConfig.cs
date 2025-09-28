using Farm.Products;

namespace Farm.Animal;

public class AnimalConfig
{
 
    private int _hunger;
    private int _health;
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
    public Milk Product { get; init; }
    public Place.Place? Place { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
  
    public string? Sound { get; init; }
    public int MaxFoodIntake { get; init; }
    public float DirtinessPerToilet { get; init; }

    public int MinHungry { get; set; }
    public int MaxHungry { get; set; }

    public int MinProductivity { get; set; }
    public int MaxProductivity { get; set; }
    
    public int ProductivityYoung { get; set; } = 30;
    public int ProductivityMiddle { get; set; } = 70;
    public int ProductivityOld { get; set; } = 20;


    public int MinHealth { get; set; }
    public int MaxHealth { get; set; }

    public int LowHungerThreshold1 { get; set; } = 30;
    public int LowHungerThreshold2 { get; set; } = 20;
    public int LowHungerThreshold3 { get; set; } = 10;

    public int ProductivityPenalty1 { get; set; } = 1;
    public int ProductivityPenalty2 { get; set; } = 3;
    public int ProductivityPenalty3 { get; set; } = 7;

    public int YoungAgeLimit { get; set; }
    public int AdultAgeLimit { get; set; }
    public int OldAgeLimit { get; set; }
}


