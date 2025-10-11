using Farm.Animals;
using Farm.Configs;
using Farm.Warehouses;
using Farm.Fields;
using Farm.Employees;
using Farm.Interfaces;
using Farm.Machines.Attachable;
using Farm.Machines.SelfPropelled;
using Farm.Places;

namespace Farm;

public static class Program
{
    private static void Main()
    {
        var warehouse = new Warehouse("Главный склад");
        var barn = new Barn("Большой амбар");

        var goat = new Goat();
        var rabbit = new Rabbit();
        var animals = new List<IAnimal> { goat, rabbit };
        foreach (var animal in animals)
        {
            animal.MoveTo(barn);
            for (int i = 0; i < 10; i++)
            {
                animal.Update();
            }
            animal.PrintStats();
            animal.Eat(10);
            animal.PrintStats();
        }

        var farmer = new Farmer(warehouse);
        farmer.MoveTo(barn);
        farmer.Work();

        var potatoField = new PotatoField();
        for (int i = 0; i < 10; i++)
        {
            potatoField.Update();
            potatoField.Fertilize();
        }

        var equipmentOperator = new EquipmentOperator(warehouse);
        var fieldWorker1 = new FieldWorker(warehouse, new EmployeeConfig
        {
            Name = "Петр",
            Age = 25,
            Level = EmployeeLevel.Junior
        });
        var fieldWorkers = new List<IWorker> { equipmentOperator, fieldWorker1 };
        foreach (var worker in fieldWorkers)
        {
            worker.MoveTo(potatoField);
        }

        var harvester = new Harvester("Harvester-23-123");
        harvester.MoveTo(potatoField);

        var tractor = new Tractor("Tractor-23-123");
        var plow = new Plow("Plow21-4");
        tractor.Attach(plow);
        tractor.MoveTo(potatoField);

        equipmentOperator.SitInMachine(harvester);
        harvester.TurnOn();
        for (int i = 0; i < 20; i++)
        {
            potatoField.Update();
        }

        fieldWorker1.ConnectToOperator(equipmentOperator);

        equipmentOperator.Work();
        fieldWorker1.Work();

        for (int i = 0; i < 210; i++)
        {
            potatoField.Update();
        }

        equipmentOperator.SitInMachine(tractor);
        tractor.TurnOn();
        equipmentOperator.Work();
        fieldWorker1.Work();

        var salesManager = new SalesManager(warehouse, new EmployeeConfig
        {
            Name = "Елена",
            Age = 31,
            Level = EmployeeLevel.Middle
        });
        salesManager.Work();

        var workers = new List<IWorker> { salesManager, equipmentOperator, farmer, fieldWorker1 };
        var accountant = new Accountant(workers, new EmployeeConfig
        {
            Name = "Ольга",
            Age = 50,
            Level = EmployeeLevel.Senior
        });
        accountant.Work();
    }
}