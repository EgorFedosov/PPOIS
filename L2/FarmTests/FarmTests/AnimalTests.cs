using Farm.Products;
using Farm.Animals;
using Farm.Places;
using Farm.Exceptions;


namespace FarmTests;

public class AnimalTest
{
    private string CaptureConsoleOutput(Action action)
    {
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        action();

        Console.SetOut(originalOut);
        return sw.ToString();
    }


    [Fact]
    public void Animal_Creation_PrintsName()
    {
        var output = CaptureConsoleOutput(() =>
        {
            var chicken = new Chicken();
        });

        Assert.Contains("Chicken", output);
    }

    [Fact]
    public void MoveTo_SamePlace_ThrowsInvalidMoveException()
    {
        var chicken = new Chicken();
        var barn = new Barn("Barn");

        chicken.MoveTo(barn);
        Assert.Throws<InvalidMoveException>(() => chicken.MoveTo(barn));
    }

    [Fact]
    public void DefaultConfig_Product_IsEgg()
    {
        var chicken = new Chicken();
        Assert.Equal(chicken.Product, new Egg());

        var barn = new Barn("Barn");

        chicken.MoveTo(barn);
        Assert.Throws<InvalidMoveException>(() => chicken.MoveTo(barn));
    }

    [Fact]
    public void PrintStats_DefaultConfig_PrintsCorrectValues()
    {
        var chicken = new Chicken();
        var output = CaptureConsoleOutput(() => { chicken.PrintStats(); });
        Assert.Contains($"Имя: {chicken.Name}", output);
        Assert.Contains($"Возраст: {chicken.Age}", output);
        Assert.Contains($"Здоровье: {chicken.Health}", output);
        Assert.Contains($"Сытость: {chicken.Hunger}", output);
        Assert.Contains($"Продуктивность: {chicken.Productivity}", output);
    }

    [Fact]
    public void MakeSound_PrintsDefaultSound()
    {
        var chicken = new Chicken();
        var output = CaptureConsoleOutput(() => { chicken.MakeSound(); });
        Assert.Contains(chicken.Sound, output);
    }

    [Fact]
    public void Die_WhenCalled_PrintsDeathMessage()
    {
        var chicken = new Chicken();
        var output = CaptureConsoleOutput(() => { chicken.Die(); });
        Assert.Contains($"{chicken.Name} умер(ла).", output);
    }

    [Fact]
    public void Eat_IncreasesHunger_AndPrintsMessage()
    {
        var chicken = new Chicken();
        var initialHunger = chicken.Hunger;
        var output = CaptureConsoleOutput(() => { chicken.Eat(10); });

        Assert.True(chicken.Hunger > initialHunger);
        Assert.Contains($"{chicken.Name} поел(а)", output);
    }

    [Fact]
    public void Update_ReducesHunger()
    {
        var chicken = new Chicken();
        var initialHunger = chicken.Hunger;

        chicken.Update();

        Assert.True(chicken.Hunger < initialHunger);
    }


    [Fact]
    public void GoToToilet_WhenPlaceIsSet_IncreasesDirtiness()
    {
        var pig = new Pig();
        var pigsty = new Barn("Pigsty");
        pig.MoveTo(pigsty);

        var initialDirtiness = pigsty.Dirtiness;
        pig.RollInMud();
        Assert.Equal(initialDirtiness + pig.DirtinessPerToilet, pigsty.Dirtiness);
    }
    
    [Fact]
public void Chicken_DigForWorms_IncreasesHunger_AndPrintsMessage()
{
    var chicken = new Chicken();
    var initialHunger = chicken.Hunger;
    var output = CaptureConsoleOutput(() => chicken.DigForWorms());

    Assert.True(chicken.Hunger > initialHunger);
    Assert.Contains(chicken.Name, output);
    Assert.Contains("копается", output);
}

[Fact]
public void Sheep_JumpInField_IncreasesProductivity_AndPrintsMessage()
{
    var sheep = new Sheep();
    var initialProductivity = sheep.Productivity;
    var output = CaptureConsoleOutput(() => sheep.JumpInField());

    Assert.True(sheep.Productivity > initialProductivity);
    Assert.Contains(sheep.Name, output);
    Assert.Contains("прыгает", output);
}

[Fact]
public void Rabbit_DigBurrow_DecreasesHunger_AndIncreasesDirtiness()
{
    var rabbit = new Rabbit();
    var field = new Barn("Field");
    rabbit.MoveTo(field);

    var initialHunger = rabbit.Hunger;
    var initialDirtiness = field.Dirtiness;
    var output = CaptureConsoleOutput(() => rabbit.DigBurrow());

    Assert.True(rabbit.Hunger < initialHunger);
    Assert.Equal(initialDirtiness + rabbit.DirtinessPerToilet, field.Dirtiness);
    Assert.Contains(rabbit.Name, output);
    Assert.Contains("роет нору", output);
}

[Fact]
public void Goat_HeadButtFence_DecreasesHealth_AndPrintsMessage()
{
    var goat = new Goat();
    var initialHealth = goat.Health;
    var output = CaptureConsoleOutput(() => goat.HeadButtFence());

    Assert.True(goat.Health < initialHealth);
    Assert.Contains(goat.Name, output);
    Assert.Contains("боднула", output);
}

[Fact]
public void Duck_Swim_IncreasesHealth_AndPrintsMessage()
{
    var duck = new Duck();
    var initialHealth = duck.Health;
    var output = CaptureConsoleOutput(() => duck.Swim());

    Assert.True(duck.Health > initialHealth);
    Assert.Contains(duck.Name, output);
    Assert.Contains("поплавала", output);
}

[Fact]
public void Cow_Graze_IncreasesHealth_AndPrintsMessage()
{
    var cow = new Cow();
    var initialHealth = cow.Health;
    var output = CaptureConsoleOutput(() => cow.Graze());

    Assert.True(cow.Health > initialHealth);
    Assert.Contains(cow.Name, output);
    Assert.Contains("отдыхает", output);
}

}