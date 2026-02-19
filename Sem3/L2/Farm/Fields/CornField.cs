using Farm.Configs;
using Farm.Products;

namespace Farm.Fields;

public class CornField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new()
    {
        Name = "Corn field",
        Product = new Corn(),
        MinProductivity = 70,
        MaxProductivity = 450,
        MinSoilCareLevel = 25
    };

    public void Weed()
    {
        Console.WriteLine($"{DefaultConfig.Name} прополото. Почва ухожена!");
        DefaultConfig.SoilCareLevel += 7;
    }
}