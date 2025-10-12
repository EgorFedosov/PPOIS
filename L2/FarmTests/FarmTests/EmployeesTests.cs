using Farm.Configs;
using Farm.Employees;
using Farm.Warehouses;
using Farm.Fields;
using Farm.Exceptions;
using Farm.Machines.SelfPropelled;
using Farm.Places;

namespace FarmTests;

public class EquipmentOperatorFieldTests
{
    private static string CaptureConsoleOutput(Action action)
    {
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        action();

        Console.SetOut(originalOut);
        return sw.ToString();
    }

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
        Assert.Contains($"{op.Name} остановил машину {op.CurrentMachine?.Name}", output);
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
    public void Work_ThrowsException_WhenSeedsInsufficient()
    {
        var warehouse = new Warehouse("Main Warehouse");
        var field = new CornField();
        var op = new EquipmentOperator(warehouse);
        var tractor = new Tractor("Tractor");
        tractor.MoveTo(field);
        tractor.TurnOn();

        op.MoveTo(field);
        op.SitInMachine(tractor);
        op.Work();
        Assert.Throws<InsufficientSeedsException>(() => op.Work());
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
        
        output =  CaptureConsoleOutput(() => acc.StopWork());
        Assert.Contains(acc.Name, output);
    }
    
    
}