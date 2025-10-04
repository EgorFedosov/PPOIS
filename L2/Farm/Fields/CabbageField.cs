using Farm.Configs;
using Farm.Products;

namespace Farm.Fields;

public class CabbageField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new()
    {
        Name = "Cabbage field",
        Product = new Cabbage(),
        MinProductivity = 30,
        MaxProductivity = 250,
        MinSoilCareLevel = 30
    };

    public void ApplyPesticide()
    {
        Console.WriteLine($"{DefaultConfig.Name} обработано от вредителей!");
        DefaultConfig.SoilCareLevel = Math.Min(100, DefaultConfig.SoilCareLevel + 10);
    }
}