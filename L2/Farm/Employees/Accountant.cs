using Farm.Configs;

namespace Farm.Employees;

public class Accountant(IEnumerable<Employee> employees, EmployeeConfig? config = null)
    : Employee(config ?? DefaultConfig)
{
    private static readonly EmployeeConfig DefaultConfig = new()
    {
        Name = "Accountant",
        Age = 30,
        Level = EmployeeLevel.Middle
    };

    private readonly EmployeeConfig _config = config ?? DefaultConfig;

    private readonly SalaryConfig _salaryConfig = new();

    public override void Work()
    {
        foreach (var employee in employees) PayEmployee(employee);

        PayEmployee(this);
    }

    private void PayEmployee(Employee employee)
    {
        var rate = _salaryConfig.GetRate(employee);
        var salary = rate * employee.GetWorkCount();
        if (salary < SalaryConfig.GetMinSalary())
            salary = SalaryConfig.GetMinSalary();

        employee.ReceiveSalary(salary);
        PromoteIfNeeded(employee);

        employee.ResetWorkCount();
        Console.WriteLine($"Начислено {salary} {employee.GetName()} ({employee.GetType().Name})");
    }

    private void PromoteIfNeeded(Employee employee)
    {
        var thresholds = _salaryConfig.GetPromotionThresholds(employee);
        var currentLevel = employee.GetLevel();
        if ((int)currentLevel < thresholds.Count && employee.GetWorkCount() >= thresholds[(int)currentLevel])
            employee.SetLevel((EmployeeLevel)((int)currentLevel + 1));
    }

    public override void StopWork()
    {
        Console.WriteLine($"{_config.Name} завершил расчёт зарплат.");
    }
}