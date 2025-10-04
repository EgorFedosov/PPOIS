using Farm.Configs;
using Farm.Products;

namespace Farm.Fields;

public class WheatField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new FieldConfig
    {
        Name = "Wheat field",
        Product = new Wheat(),
        MinProductivity = 80,
        MaxProductivity = 500,
        MinSoilCareLevel = 15,
        SoilCareDecrement = 4
    };

    public void Irrigate()
    {
        Console.WriteLine($"{DefaultConfig.Name} поливается. Рост ускорен!");
        DefaultConfig.Productivity += 20;
    }
}