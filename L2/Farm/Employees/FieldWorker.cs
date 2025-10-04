using Farm.Configs;
using Farm.Fields;
using Farm.Machines.Attachable;
using Farm.Machines.SelfPropelled;
using Farm.Warehouses;

namespace Farm.Employees;

public class FieldWorker(Warehouse warehouse, EmployeeConfig? config = null)
    : EmployeeWithWarehouse(config ?? DefaultConfig, warehouse)
{
    private static readonly EmployeeConfig DefaultConfig = new()
    {
        Name = "Field Worker",
        Age = 25
    };

    private readonly EmployeeConfig _config = config ?? DefaultConfig;

    private EquipmentOperator? PartnerOperator { get; set; }
    private Tractor? CurrentTractor => PartnerOperator?.CurrentMachine as Tractor;

    public override void Work()
    {
        if (_config.Location is not Field field)
            throw new InvalidOperationException("Работник должен находиться на поле!");

        if (PartnerOperator != null)
        {
            if (CurrentTractor == null)
                throw new InvalidOperationException("У оператора нет техники!");

            Console.WriteLine($"{_config.Name} работает на тракторе {CurrentTractor.Name}.");
            AssistOperator();
        }
        else
        {
            Console.WriteLine($"{_config.Name} выполняет ручные работы на поле {field.Name}.");
            ManualWork(field);
        }

        _config.WorkCount++;
    }

    public void ConnectToOperator(EquipmentOperator operatorWorker)
    {
        PartnerOperator = operatorWorker;
        Console.WriteLine($"🔗 {_config.Name} теперь работает в паре с {operatorWorker.Name ?? "оператором"}.");
    }

    private void DisconnectOperator()
    {
        Console.WriteLine($"{_config.Name} больше не работает с оператором техники.");
        PartnerOperator = null;
    }

    private void AssistOperator()
    {
        if (CurrentTractor == null) return;

        foreach (var attachment in CurrentTractor.Attachments)
            switch (attachment)
            {
                case Plow plow:
                    plow.PlowField();
                    break;
                case Seeder seeder:
                    seeder.SeedField();
                    break;
                case CropSprayer sprayer:
                    sprayer.SprayField();
                    break;
                default:
                    Console.WriteLine("Неизвестное навесное оборудование.");
                    break;
            }
    }

    private void ManualWork(Field field)
    {
        if (!field.CollectProduct(Warehouse) && !field.TryPlantFromWarehouse(Warehouse))
            Console.WriteLine("Недостаточно семян или место на поле заполнено.");
    }

    public override void StopWork()
    {
        DisconnectOperator();
        Console.WriteLine($"{_config.Name} завершил работу на поле и идёт отдыхать.");
    }
}