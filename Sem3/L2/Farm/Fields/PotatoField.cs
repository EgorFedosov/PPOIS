using Farm.Configs;
using Farm.Products;

namespace Farm.Fields;

public class PotatoField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new()
    {
        Name = "Potato field",
        Product = new Potato(),
        MinProductivity = 50,
        MaxProductivity = 300,
        MinSoilCareLevel = 20
    };

    public void Fertilize()
    {
        Console.WriteLine($"{DefaultConfig.Name} удобряется. Производительность увеличена!");
        DefaultConfig.SoilCareLevel += 5;
    }
}