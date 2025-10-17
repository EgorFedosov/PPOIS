
using Farm.Configs;

using Farm.Products;

namespace FarmTests;

public class ProductTests
{
    [Fact]
public void Product_Update_ReducesFreshnessAndAmount_WhenDamageIsHigh()
{
    var config = new ProductConfig
    {
        Amount = 10,
        Freshness = 100,
        BasePrice = 10,
        MaxAmount = 20,
        Damage = 90,
        DamageLevel1 = 20,
        DamageLevel2 = 30,
        DamageLevel3 = 50
    };

    var wool = new Wool(config);
    wool.Update();

    Assert.Equal(0, wool.Freshness);
    Assert.Equal(0, wool.Amount);
}

[Fact]
public void Wool_HandleAfterCollection_DoesNothing()
{
    var wool = new Wool();
    var initialFreshness = wool.Freshness;
    var initialAmount = wool.Amount;

    wool.HandleAfterCollection();

    Assert.Equal(initialFreshness, wool.Freshness);
    Assert.Equal(initialAmount, wool.Amount);
}

[Fact]
public void Corn_HandleAfterCollection_DoesNothing()
{
    var corn = new Corn();
    var initialFreshness = corn.Freshness;
    var initialAmount = corn.Amount;

    corn.HandleAfterCollection();

    Assert.Equal(initialFreshness - 3, corn.Freshness);
    Assert.Equal(initialAmount, corn.Amount);
}

[Fact]
public void CropSeed_HandleAfterCollection_DoesNothing()
{
    var seed = new CropSeed();
    var initialFreshness = seed.Freshness;
    var initialAmount = seed.Amount;

    seed.HandleAfterCollection();

    Assert.Equal(initialFreshness, seed.Freshness);
    Assert.Equal(initialAmount, seed.Amount);
}

[Fact]
public void Wheat_HandleAfterCollection_DoesNothing()
{
    var wheat = new Wheat();
    var initialFreshness = wheat.Freshness;
    var initialAmount = wheat.Amount;

    wheat.HandleAfterCollection();

    Assert.Equal(initialFreshness, wheat.Freshness);
    Assert.Equal(initialAmount, wheat.Amount);
}

[Fact]
public void Fruit_HandleAfterCollection_DoesNothing()
{
    var fruit = new Fruit();
    var initialFreshness = fruit.Freshness;
    var initialAmount = fruit.Amount;

    fruit.HandleAfterCollection();

    Assert.Equal(initialFreshness, fruit.Freshness);
    Assert.Equal(initialAmount, fruit.Amount);
}


}