using Farm.Configs;
using Farm.Products;

namespace Farm.Fields;

public class FruitField(FieldConfig? config = null) : Field(config ?? DefaultConfig)
{
    private static readonly FieldConfig DefaultConfig = new()
    {
        Name = "Fruit field",
        Product = new Fruit(),
        MinProductivity = 40,
        MaxProductivity = 600,
        MinSoilCareLevel = 35
    };

    public void Prune()
    {
        Console.WriteLine($"{DefaultConfig.Name} обрезается. Плоды станут крупнее!");
        DefaultConfig.Productivity += 15;
    }
}