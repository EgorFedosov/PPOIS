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
    private decimal _balance = 0;

    public decimal Balance
    {
        get => _balance;
        set => _balance = value < 0 ? 0 : value;
    }

    private readonly int _age;
    private int _workCount;

    private const int MinAge = 18;
    private const int MaxAge = 120;


    public string Name { get; init; }

    public int Age
    {
        get => _age;
        init => _age = Math.Clamp(value, MinAge, MaxAge);
    }

    public EmployeeLevel Level { get; set; } = EmployeeLevel.Intern;

    public Place? Location { get; set; }

    public decimal Salary { get; set; }

    public int WorkCount
    {
        get => _workCount;
        set => _workCount = value < 0 ? 0 : value;
    }
}