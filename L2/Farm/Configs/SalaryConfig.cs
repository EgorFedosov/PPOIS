namespace Farm.Configs;

public class SalaryConfig
{
    private const decimal MinSalary = 2000;

    public Dictionary<string, EmployeeLimits> LimitsByType { get; init; } = new()
    {
        ["Accountant"] = new EmployeeLimits { InternLimit = 80, JuniorLimit = 200, MiddleLimit = 400, SeniorLimit = 700 },
        ["EquipmentOperator"] = new EmployeeLimits { InternLimit = 120, JuniorLimit = 350, MiddleLimit = 700, SeniorLimit = 1200 },
        ["Farmer"] = new EmployeeLimits { InternLimit = 100, JuniorLimit = 300, MiddleLimit = 600, SeniorLimit = 1000 },
        ["FieldWorker"] = new EmployeeLimits { InternLimit = 90, JuniorLimit = 250, MiddleLimit = 500, SeniorLimit = 900 },
        ["Mechanic"] = new EmployeeLimits { InternLimit = 60, JuniorLimit = 180, MiddleLimit = 350, SeniorLimit = 600 },
        ["SalesManager"] = new EmployeeLimits { InternLimit = 70, JuniorLimit = 220, MiddleLimit = 450, SeniorLimit = 800 },
        ["Security"] = new EmployeeLimits { InternLimit = 110, JuniorLimit = 320, MiddleLimit = 650, SeniorLimit = 1100 },
        ["Veterinarian"] = new EmployeeLimits { InternLimit = 50, JuniorLimit = 150, MiddleLimit = 300, SeniorLimit = 500 }
    };
}
