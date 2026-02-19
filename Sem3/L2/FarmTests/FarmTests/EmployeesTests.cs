using Farm.Animals;
using Farm.Configs;
using Farm.Employees;
using Farm.Warehouses;
using Farm.Fields;
using Farm.Exceptions;
using static FarmTests.TestUtils;
using Farm.Machines.SelfPropelled;
using Farm.Places;
using Farm.Products;

namespace FarmTests;

public class EquipmentOperatorFieldTests
{
    [Fact]
    public void Operator_Creation_PrintsName()
    {
        EquipmentOperator? op = null;
        var output = CaptureConsoleOutput(() => op = new EquipmentOperator(new Warehouse("Main Warehouse")));
        Assert.Contains(op.Name, output);
    }

    [Fact]
    public void SitInMachine_AssignsDriverToMachine()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);
        op.MoveTo(field);

        op.SitInMachine(tractor);

        Assert.Equal(op, tractor.Driver);
    }

    [Fact]
    public void SitInMachine_AssignsMachineToOperator()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);
        op.MoveTo(field);

        op.SitInMachine(tractor);

        Assert.Equal(tractor, op.CurrentMachine);
    }

    [Fact]
    public void SitInMachine_PrintsOperatorAndMachineNames()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);
        op.MoveTo(field);

        var output = CaptureConsoleOutput(() => op.SitInMachine(tractor));

        Assert.Contains(op.Name, output);
        Assert.Contains(tractor.Name, output);
    }

    [Fact]
    public void CheckChangeLevelAndDefaultWorkCount()
    {
        var warehouse = new Warehouse("Main Warehouse");

        var op = new EquipmentOperator(warehouse);

        op.Level = EmployeeLevel.Senior;
        op.WorkCount();

        Assert.Equal(EmployeeLevel.Senior, op.Level);
        Assert.Equal(0, op.WorkCount());
    }


    [Fact]
    public void SitInMachine_Throws_WhenMachineOccupied()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op1 = new EquipmentOperator(warehouse);
        var op2 = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);

        op1.MoveTo(field);
        op1.SitInMachine(tractor);

        op2.MoveTo(field);
        Assert.Throws<MachineAlreadyOccupiedException>(() => op2.SitInMachine(tractor));
    }


    [Fact]
    public void StopWork_TurnsOffMachine_AndClearsDriver_AndPrintsMessages()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);
        tractor.TurnOn();

        op.MoveTo(field);
        op.SitInMachine(tractor);
        var output = CaptureConsoleOutput(() => op.StopWork());

        Assert.Null(op.CurrentMachine);
        Assert.Null(tractor.Driver);
        Assert.False(tractor.IsOn);
        Assert.Contains($"{op.Name} остановил машину {tractor.Name}", output);
        Assert.Contains("завершил работу", output);
    }


    [Fact]
    public void Work_Throws_WhenMachineOrFieldNotAssigned()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var op = new EquipmentOperator(warehouse);
        Assert.Throws<EquipmentOrFieldNotAssignedException>(() => op.Work());
    }


    [Fact]
    public void Work_Throws_WhenMachineIsOff()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CabbageField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);


        op.MoveTo(field);
        op.SitInMachine(tractor);
        Assert.Throws<MachineNotOnException>(() => op.Work());
    }

    [Fact]
    public void Work_Throws_WhenLocationIsNotField()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var noField = new Barn("NoField");
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(noField);
        tractor.TurnOn();

        op.MoveTo(noField);
        op.SitInMachine(tractor);
        Assert.Throws<InvalidWorkLocationException>(() => op.Work());
    }

  

    [Fact]
    public void Accountant_PrintsEmployeeAndSelf_WhenWorkAndStopWork()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var op = new EquipmentOperator(warehouse);
        var acc = new Accountant([op]);

        var output = CaptureConsoleOutput(() => acc.Work());
        Assert.Contains(op.Name, output);
        Assert.Contains(acc.Name, output);

        output = CaptureConsoleOutput(() => acc.StopWork());
        Assert.Contains(acc.Name, output);
    }


[Fact]
public void Farmer_Creation_UsesDefaultConfig()
{
    var warehouse = new Warehouse("Main Warehouse");
    var farmer = new Farmer(warehouse);

    Assert.Equal("Farmer", farmer.Name);
    Assert.Equal(25, farmer.Age);
    Assert.Equal(EmployeeLevel.Junior, farmer.Level);
}

[Fact]
public void Work_Throws_NoAnimalsOnLocationException_WhenLocationHasNoAnimals()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("EmptyBarn");
    var farmer = new Farmer(warehouse);
    farmer.MoveTo(barn);

    Assert.Throws<NoAnimalsOnLocationException>(() => farmer.Work());
}


[Fact]
public void Work_PrintsCorrectMessage_ForGoat()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("GoatBarn");
    barn.AddEntity(new Goat());
    var farmer = new Farmer(warehouse);
    farmer.MoveTo(barn);

    var output = CaptureConsoleOutput(() => farmer.Work());

    Assert.Contains("Farmer собрал Milk у Goat", output);
}

[Fact]
public void Work_PrintsButcheringMessage_ForPig()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("PigBarn");
    barn.AddEntity(new Pig());
    var farmer = new Farmer(warehouse);
    farmer.MoveTo(barn);

    var output = CaptureConsoleOutput(() => farmer.Work());

    Assert.Contains("Pig был забит на мясо...", output);
}


[Fact]
public void Work_IncrementsWorkCount_AfterSuccessfulExecution()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("GoatBarn");
    barn.AddEntity(new Goat());
    var farmer = new Farmer(warehouse);
    farmer.MoveTo(barn);

    var initialCount = farmer.WorkCount();
    farmer.Work();
    var finalCount = farmer.WorkCount();

    Assert.Equal(initialCount + 1, finalCount);
}


[Fact]
public void Work_MakesNoSounds_WhenFarmerIsSenior()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("GoatBarn");
    barn.AddEntity(new Goat());
    var config = new EmployeeConfig
    {
        Name = "SeniorFarmer",
        Age = 40,
        Level = EmployeeLevel.Senior,
        Location = barn
    };
    var farmer = new Farmer(warehouse, config);

    var output = CaptureConsoleOutput(() => farmer.Work());

    Assert.DoesNotContain("Baa!", output);
}

[Fact]
public void FieldWorker_Creation_UsesDefaultConfig()
{
    var warehouse = new Warehouse("Main Warehouse");
    var worker = new FieldWorker(warehouse);

    Assert.Equal("Field Worker", worker.Name);
    Assert.Equal(25, worker.Age);
}

[Fact]
public void Work_Throws_FieldWorkerNotOnFieldException_WhenLocationIsNotField()
{
    var warehouse = new Warehouse("Main Warehouse");
    var barn = new Barn("NotAField");
    var worker = new FieldWorker(warehouse);
    worker.MoveTo(barn);

    Assert.Throws<FieldWorkerNotOnFieldException>(() => worker.Work());
}

[Fact]
public void Work_PerformsManualWork_WhenNoPartnerOperator()
{
    var warehouse = new Warehouse("Main Warehouse");
    var field = new CornField();
    var worker = new FieldWorker(warehouse);
    worker.MoveTo(field);

    var output = CaptureConsoleOutput(() => worker.Work());

    Assert.Contains("Field Worker выполняет ручные работы на поле", output);
}

[Fact]
public void ConnectToOperator_AssignsPartner_AndPrintsMessage()
{
    var warehouse = new Warehouse("Main Warehouse");
    var op = new EquipmentOperator(warehouse);
    var worker = new FieldWorker(warehouse);

    var output = CaptureConsoleOutput(() => worker.ConnectToOperator(op));

    Assert.Equal(op, worker.PartnerOperator);
    Assert.Contains("Field Worker теперь работает в паре с", output);
    Assert.Contains(op.Name, output);
}



[Fact]
public void ManualWork_TriesToCollectAndPlant_WhenNoPartner()
{
    var warehouse = new Warehouse("Main Warehouse");
    var field = new CornField();
    var worker = new FieldWorker(warehouse);
    worker.MoveTo(field);

    var output = CaptureConsoleOutput(() => worker.Work());

    // Если поле пустое и нет семян — выведется сообщение
    Assert.Contains("Недостаточно семян или место на поле заполнено.", output);
}

[Fact]
public void StopWork_DisconnectsOperator_AndPrintsMessage()
{
    var warehouse = new Warehouse("Main Warehouse");
    var op = new EquipmentOperator(warehouse);
    var worker = new FieldWorker(warehouse);
    worker.ConnectToOperator(op);

    var output = CaptureConsoleOutput(() => worker.StopWork());

    Assert.Null(worker.PartnerOperator);
    Assert.Contains("Field Worker больше не работает с оператором техники.", output);
    Assert.Contains("завершил работу на поле", output);
}

[Fact]
public void SalesManager_Creation_UsesDefaultConfig()
{
    var warehouse = new Warehouse("Main Warehouse");
    var manager = new SalesManager(warehouse);

    Assert.Equal("Sales Manager", manager.Name);
    Assert.Equal(28, manager.Age);
    Assert.Equal(EmployeeLevel.Middle, manager.Level);
}



[Fact]
public void StopWork_PrintsTotalEarning()
{
    var warehouse = new Warehouse("Main Warehouse");
    var manager = new SalesManager(warehouse);

    var output = CaptureConsoleOutput(() => manager.StopWork());

    Assert.Contains("Sales Manager завершил работу. Всего заработано: 0", output);
}




[Fact]
public void AssistOperator_DoesNothing_WhenNoAttachments()
{
    var warehouse = new Warehouse("Main Warehouse");
    var field = new CornField();
    var op = new EquipmentOperator(warehouse);
    var worker = new FieldWorker(warehouse);
    var tractor = new Tractor("Tractor");
    tractor.MoveTo(field);
    op.MoveTo(field);
    op.SitInMachine(tractor);
    worker.PartnerOperator = op;

    var output = CaptureConsoleOutput(() => worker.AssistOperator());

    // Нет вывода от оборудования → пустой вывод или только от других частей (но их нет)
    Assert.DoesNotContain("PlowField", output);
    Assert.DoesNotContain("SeedField", output);
    Assert.DoesNotContain("SprayField", output);
}



[Fact]
public void Sell_ReturnsRevenue_WhenProductIsAvailable()
{
    var warehouse = new Warehouse("Main Warehouse");
    var potato = new Potato();
    warehouse.Store(potato);

    var manager = new SalesManager(warehouse);
    var revenue = manager.Sell(potato, (uint)potato.Amount);

    Assert.Equal(potato.Price * (uint)potato.Amount, revenue);
}

[Fact]
public void Sell_PrintsSuccessMessage_WhenProductIsAvailable()
{
    var warehouse = new Warehouse("Main Warehouse");
    var cabbage = new Cabbage();
    warehouse.Store(cabbage);

    var manager = new SalesManager(warehouse);
    var output = CaptureConsoleOutput(() => manager.Sell(cabbage, (uint)cabbage.Amount));

    Assert.Contains("Продано", output);
    Assert.Contains("Cabbage", output);
    Assert.Contains("выручка", output);
}

[Fact]
public void Sell_ReturnsZero_WhenProductNotAvailable()
{
    var warehouse = new Warehouse("Main Warehouse");
    var egg = new Egg();

    var manager = new SalesManager(warehouse);
    var revenue = manager.Sell(egg, 1);

    Assert.Equal(0, revenue);
}

[Fact]
public void Sell_PrintsFailureMessage_WhenProductNotAvailable()
{
    var warehouse = new Warehouse("Main Warehouse");
    var corn = new Corn();

    var manager = new SalesManager(warehouse);
    var output = CaptureConsoleOutput(() => manager.Sell(corn, 1));

    Assert.Contains("Не удалось продать", output);
    Assert.Contains("Corn", output);
}

[Fact]
public void Work_IncrementsWorkCount_AfterExecution()
{
    var warehouse = new Warehouse("Main Warehouse");
    var potato = new Potato();
    warehouse.Store(potato);

    var manager = new SalesManager(warehouse);
    var initialCount = manager.WorkCount();
    manager.Work();
    var finalCount = manager.WorkCount();

    Assert.Equal(initialCount + 1, finalCount);
}

[Fact]
public void StopWork_PrintsTotalEarnings()
{
    var warehouse = new Warehouse("Main Warehouse");
    var egg = new Egg();
    warehouse.Store(egg);

    var manager = new SalesManager(warehouse);
    manager.Work(); // чтобы заработать деньги

    var output = CaptureConsoleOutput(() => manager.StopWork());

    Assert.Contains("Sales Manager завершил работу. Всего заработано:", output);
    Assert.Contains("денег", output);
}

}