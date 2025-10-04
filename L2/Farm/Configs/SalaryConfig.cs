using Farm.Employees;

namespace Farm.Configs;

public class SalaryConfig
{
    private const decimal MinSalary = 2000m;

    private Dictionary<string, EmployeeSalaryData> LimitsByType { get; } = new()
    {
        ["Accountant"] = new EmployeeSalaryData
        {
            Rates = new Dictionary<EmployeeLevel, int>
            {
                [EmployeeLevel.Intern] = 80,
                [EmployeeLevel.Junior] = 200,
                [EmployeeLevel.Middle] = 400,
                [EmployeeLevel.Senior] = 700
            },
            PromotionThresholds = [50, 150, 300]
        },
        ["EquipmentOperator"] = new EmployeeSalaryData
        {
            Rates = new Dictionary<EmployeeLevel, int>
            {
                [EmployeeLevel.Intern] = 120,
                [EmployeeLevel.Junior] = 350,
                [EmployeeLevel.Middle] = 700,
                [EmployeeLevel.Senior] = 1200
            },
            PromotionThresholds = [40, 120, 250]
        },
        ["Farmer"] = new EmployeeSalaryData
        {
            Rates = new Dictionary<EmployeeLevel, int>
            {
                [EmployeeLevel.Intern] = 100,
                [EmployeeLevel.Junior] = 300,
                [EmployeeLevel.Middle] = 600,
                [EmployeeLevel.Senior] = 1000
            },
            PromotionThresholds = [30, 100, 200]
        },
        ["FieldWorker"] = new EmployeeSalaryData
        {
            Rates = new Dictionary<EmployeeLevel, int>
            {
                [EmployeeLevel.Intern] = 90,
                [EmployeeLevel.Junior] = 250,
                [EmployeeLevel.Middle] = 500,
                [EmployeeLevel.Senior] = 900
            },
            PromotionThresholds = [25, 80, 160]
        },
        ["SalesManager"] = new EmployeeSalaryData
        {
            Rates = new Dictionary<EmployeeLevel, int>
            {
                [EmployeeLevel.Intern] = 70,
                [EmployeeLevel.Junior] = 220,
                [EmployeeLevel.Middle] = 450,
                [EmployeeLevel.Senior] = 800
            },
            PromotionThresholds = [20, 70, 150]
        }
    };

    public decimal GetRate(Employee employee)
    {
        var typeName = employee.GetType().Name;
        return !LimitsByType.TryGetValue(typeName, out var data)
            ? 0
            : data.Rates[employee.GetLevel()];
    }

    public List<int> GetPromotionThresholds(Employee employee)
    {
        var typeName = employee.GetType().Name;
        return !LimitsByType.TryGetValue(typeName, out var data) ? [] : data.PromotionThresholds;
    }

    public static decimal GetMinSalary()
    {
        return MinSalary;
    }
}

public class EmployeeSalaryData
{
    public Dictionary<EmployeeLevel, int> Rates { get; init; } = new();
    public List<int> PromotionThresholds { get; init; } = [];
}