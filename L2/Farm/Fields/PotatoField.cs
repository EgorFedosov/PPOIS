using Farm.Configs;
using Farm.Products;
using Farm.Configs;
namespace Farm.Fields;

public class PotatoField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new FieldConfig
    {
        Name = "Potato field",
        Product = new Potato(),
        MinProductivity = 50,
        MaxProductivity = 300,
        MinSoilCareLevel = 20,
        SoilCareDecrement = 5
    };

    public void Fertilize()
    {
        Console.WriteLine($"{DefaultConfig.Name} удобряется. Производительность увеличена!");
        DefaultConfig.SoilCareLevel += 5;
    }
}