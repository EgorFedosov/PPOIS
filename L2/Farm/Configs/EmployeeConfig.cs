using Farm.Places;

namespace Farm.Configs;

public enum EmployeeLevel
{
    Intern,
    Junior,
    Middle,
    Senior
}

public class EmployeeConfig
{
    private const int MinAge = 18;
    private const int MaxAge = 120;

    private readonly int _age;
    private decimal _balance;
    private int _workCount;

    public decimal Balance
    {
        get => _balance;
        set => _balance = value < 0 ? 0 : value;
    }


    public string? Name { get; init; }

    public int Age
    {
        get => _age;
        init => _age = Math.Clamp(value, MinAge, MaxAge);
    }

    public EmployeeLevel Level { get; set; } = EmployeeLevel.Intern;

    public Place? Location { get; set; }


    public int WorkCount
    {
        get => _workCount;
        set => _workCount = value < 0 ? 0 : value;
    }
}