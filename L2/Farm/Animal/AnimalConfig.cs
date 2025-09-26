namespace Farm.Animal;

public class AnimalConfig
{
    public int MaxHealth { get; init; } = 100;
    public int MinHealth { get; init; } = 0;
    public int MaxHungry { get; init; } = 100;
    public int MinHungry { get; init; } = 0;
    public int MaxProductivity { get; init; } = 100;
    public int MinProductivity { get; init; } = 0;

    public int MaxFoodIntake { get; init; }
    public float DirtinessPerToilet { get; init; }
    public string Sound { get; init; } = null!; 

    public int YoungAgeLimit { get; init; }
    public int AdultAgeLimit { get; init; }
    public int OldAgeLimit { get; init; }
}

