using Farm.Employees;
using Farm.Exceptions;
using Farm.Fields;
using Farm.Machines.Attachable;
using Farm.Machines.SelfPropelled;
using Farm.Warehouses;

namespace FarmTests;

public class MachineTests
{
    private static string CaptureConsoleOutput(Action action)
    {
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            action();
            return sw.ToString();
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void Tractor_Attach_AddsAttachmentToAttachmentsList()
    {
        var tractor = new Tractor("BigTractor");
        var plow = new Plow("DeepPlow");

        tractor.Attach(plow);

        Assert.Contains(plow, tractor.Attachments);
    }


    [Fact]
    public void Tractor_Detach_RemovesAttachmentFromAttachmentsList()
    {
        var tractor = new Tractor("T2");
        var sprayer = new CropSprayer("SprayMax");
        tractor.Attach(sprayer);

        tractor.Detach(sprayer);

        Assert.DoesNotContain(sprayer, tractor.Attachments);
    }


    [Fact]
    public void Attach_PrintsMessage_ContainingAttachmentAndTractorNames()
    {
        var tractor = new Tractor("RedTractor");
        var seeder = new Seeder("AutoSeeder");

        var output = CaptureConsoleOutput(() => tractor.Attach(seeder));

        Assert.Contains(seeder.Name, output);
        Assert.Contains(tractor.Name, output);
    }

    [Fact]
    public void Detach_PrintsMessage_ContainingAttachmentAndTractorNames()
    {
        var tractor = new Tractor("GreenTractor");
        var sprayer = new CropSprayer("MegaSpray");
        tractor.Attach(sprayer);

        var output = CaptureConsoleOutput(() => tractor.Detach(sprayer));

        Assert.Contains(sprayer.Name, output);
    }

    [Fact]
    public void Tractor_CanAttachMultipleAttachments()
    {
        var tractor = new Tractor("MultiTractor");
        var plow = new Plow("P");
        var seeder = new Seeder("S");
        var sprayer = new CropSprayer("Spr");

        tractor.Attach(plow);
        tractor.Attach(seeder);
        tractor.Attach(sprayer);

        Assert.Equal(3, tractor.Attachments.Count);
        Assert.Contains(plow, tractor.Attachments);
        Assert.Contains(seeder, tractor.Attachments);
        Assert.Contains(sprayer, tractor.Attachments);
    }

    [Fact]
    public void Harvester_Creation_SetsDefaultName_WhenNullProvided()
    {
        var harvester = new Harvester(null);
        Assert.Equal("Harvester", harvester.Name);
    }

    [Fact]
    public void Harvester_Creation_UsesProvidedName()
    {
        var harvester = new Harvester("BigHarvester");
        Assert.Equal("BigHarvester", harvester.Name);
    }

    [Fact]
    public void AssignDriver_Throws_InvalidHarvesterDriverException_WhenDriverIsNotEquipmentOperator()
    {
        var harvester = new Harvester("H1");
        var farmer = new Farmer(new Warehouse("W"));

        Assert.Throws<InvalidHarvesterDriverException>(() => harvester.AssignDriver(farmer));
    }

    [Fact]
    public void AssignDriver_Succeeds_WhenDriverIsEquipmentOperator()
    {
        var harvester = new Harvester("H2");
        var op = new EquipmentOperator(new Warehouse("W"));

        harvester.AssignDriver(op);

        Assert.Equal(op, harvester.Driver);
    }
    

    [Fact]
    public void TurnOn_Succeeds_WhenDriverIsEquipmentOperator()
    {
        var harvester = new Harvester("H5");
        var op = new EquipmentOperator(new Warehouse("W"));
        harvester.AssignDriver(op);

        var output = CaptureConsoleOutput(() => harvester.TurnOn());

        Assert.True(harvester.IsOn);
        Assert.Contains("H5 завелся(ась)", output);
    }

    [Fact]
    public void DriveTo_Throws_NoDriverAssignedException_WhenNoDriver()
    {
        var harvester = new Harvester("H6");
        harvester.TurnOn(); // но без драйвера
        var field = new CornField();

        Assert.Throws<NoDriverAssignedException>(() => harvester.DriveTo(field));
    }

    [Fact]
    public void DriveTo_Throws_MachineNotOnException_WhenOff()
    {
        var harvester = new Harvester("H7");
        var op = new EquipmentOperator(new Warehouse("W"));
        harvester.AssignDriver(op);
        var field = new CornField();

        Assert.Throws<MachineNotOnException>(() => harvester.DriveTo(field));
    }

    [Fact]
    public void DriveTo_MovesHarvesterToDestination_WhenOnAndHasDriver()
    {
        var harvester = new Harvester("H8");
        var op = new EquipmentOperator(new Warehouse("W"));
        harvester.AssignDriver(op);
        harvester.TurnOn();
        var field = new CornField();

        harvester.DriveTo(field);

        Assert.Equal(field, harvester.Location);
    }
}