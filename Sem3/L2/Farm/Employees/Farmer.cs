using Farm.Animals;
using Farm.Configs;
using Farm.Interfaces;
using Farm.Warehouses;
using Farm.Exceptions;

namespace Farm.Employees;

public class Farmer(Warehouse warehouse, EmployeeConfig? config = null)
    : EmployeeWithWarehouse(config ?? DefaultConfig, warehouse)
{
    private static readonly EmployeeConfig DefaultConfig = new()
    {
        Name = "Farmer",
        Age = 25,
        Level = EmployeeLevel.Junior
    };

    private readonly EmployeeConfig _config = config ?? DefaultConfig;

    public override void Work()
    {
        if (_config.Location == null)
            throw new FarmerLocationNotAssignedException("Фермер не на месте");

        var animals = _config.Location.GetAnimals().ToList();
        if (animals.Count == 0)
            throw new NoAnimalsOnLocationException("Нет животных для работы");

        foreach (var animal in animals) CollectFromAnimal(animal);

        _config.WorkCount++;
    }

    private void CollectFromAnimal(IAnimal animal)
    {
        var product = animal.Product;
        if (product == null)
            throw new AnimalHasNoProductException($"{animal.GetType().Name} не производит продукт");
        ReactToFarmerLevel(animal);

        if (animal is Pig or Rabbit)
        {
            Console.WriteLine($"{animal.GetType().Name} был забит на мясо...");
            Warehouse.Store(product);
            animal.Die();
            return;
        }

        Warehouse.Store(product);
        Console.WriteLine($"{_config.Name} собрал {product.GetType().Name} у {animal.GetType().Name}.");
    }

    private void ReactToFarmerLevel(IAnimal animal)
    {
        var noiseLevel = _config.Level switch
        {
            EmployeeLevel.Intern => 3,
            EmployeeLevel.Junior => 2,
            EmployeeLevel.Middle => 1,
            EmployeeLevel.Senior => 0,
            _ => 2
        };

        if (noiseLevel > 0)
            for (var i = 0; i < noiseLevel; i++)
                animal.MakeSound();
    }


    public override void StopWork()
    {
        Console.WriteLine($" {_config.Name} завершил сбор продуктов и покинул ферму.");
        _config.Location = null;
    }
}