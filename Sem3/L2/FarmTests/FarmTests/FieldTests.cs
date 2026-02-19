using Farm.Fields;
using Farm.Warehouses;

namespace FarmTests;

public class FieldTests
{
    [Fact]
    public void CabbageField_ApplyPesticide_IncreasesSoilCareLevel()
    {
        var field = new CabbageField();
        var initialCare = field.SoilCareLevel;

        field.ApplyPesticide();

        Assert.Equal(initialCare + 30, field.SoilCareLevel);
    }

    [Fact]
    public void CornField_Fertilize_IncreasesSoilCareLevel()
    {
        var field = new CornField();
        var initialCare = field.SoilCareLevel;

        field.Weed();

        Assert.Equal(initialCare + 25, field.SoilCareLevel);
    }

    [Fact]
    public void WheatField_RotateCrop_IncreasesSoilCareLevel()
    {
        var field = new WheatField();
        var initialCare = field.SoilCareLevel;

        field.Irrigate();

        Assert.Equal(initialCare, field.SoilCareLevel);
    }

    [Fact]
    public void PotatoField_HillUp_IncreasesSoilCareLevel()
    {
        var field = new PotatoField();
        var initialCare = field.SoilCareLevel;

        field.Fertilize();

        Assert.Equal(initialCare + 20, field.SoilCareLevel);
    }

    [Fact]
    public void FruitField_PruneTrees_IncreasesSoilCareLevel()
    {
        var field = new FruitField();
        var initialCare = field.SoilCareLevel;

        field.Prune();

        Assert.Equal(initialCare, field.SoilCareLevel);
    }

    [Fact]
    public void CabbageField_TryPlantFromWarehouse_PlantsCabbage_WhenSeedsAvailable()
    {
        var warehouse = new Warehouse("W");
        var field = new CabbageField();

        var result = field.TryPlantFromWarehouse(warehouse);

        Assert.True(result);
    }

    [Fact]
    public void WheatField_TryPlantFromWarehouse_PlantsWheat_WhenSeedsAvailable()
    {
        var warehouse = new Warehouse("W");
        var field = new WheatField();

        var result = field.TryPlantFromWarehouse(warehouse);

        Assert.True(result);
    }

    [Fact]
    public void PotatoField_TryPlantFromWarehouse_PlantsPotato_WhenSeedsAvailable()
    {
        var warehouse = new Warehouse("W");
        var field = new PotatoField();

        var result = field.TryPlantFromWarehouse(warehouse);

        Assert.True(result);
    }
}