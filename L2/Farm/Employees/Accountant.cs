using Farm.Configs;
using Farm.Interfaces;

namespace Farm.Employees;

public class Accountant(IEnumerable<IWorker> employees, EmployeeConfig? config = null)
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

    private void PayEmployee(IWorker employee)
    {
        var rate = _salaryConfig.GetRate(employee);
        var salary = rate * employee.WorkCount();
        if (salary < SalaryConfig.GetMinSalary())
            salary = SalaryConfig.GetMinSalary();

        employee.ReceiveSalary(salary);
        PromoteIfNeeded(employee);

        employee.ResetWorkCount();
        Console.WriteLine($"Начислено {salary} {employee.Name} ({employee.GetType().Name})");
    }

    private void PromoteIfNeeded(IWorker employee)
    {
        var thresholds = _salaryConfig.GetPromotionThresholds(employee);
        var currentLevel = employee.Level;
        if ((int)currentLevel < thresholds.Count && employee.WorkCount() >= thresholds[(int)currentLevel])
            employee.Level = ((EmployeeLevel)((int)currentLevel + 1));
    }

    public override void StopWork()
    {
        Console.WriteLine($"{_config.Name} завершил расчёт зарплат.");
    }
}