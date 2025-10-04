using Farm.Configs;
using Farm.Interfaces;
using Farm.Places;
using Farm.Products;

namespace Farm.Animals;

public abstract class Animal(AnimalConfig config)
    : IAnimal
{
    public Product? Product => config.Product;

    public void PrintStats()
    {
        Console.WriteLine($"Имя: {config.Name}");
        Console.WriteLine($"Возраст: {config.Age}");
        Console.WriteLine($"Здоровье: {config.Health}");
        Console.WriteLine($"Сытость: {config.Hunger}");
        Console.WriteLine($"Продуктивность: {config.Productivity}");
    }

    public void MakeSound()
    {
        Console.WriteLine(config.Sound);
    }

    public void Die()
    {
        Console.WriteLine($"{config.Name} умер(ла).");
        config.Place?.RemoveEntity(this);
        config.Health = 0;
    }

    public void MoveTo(Place? newPlace)
    {
        if (newPlace == null || config.Place == newPlace) return;
        //TODO добавить кастомное исключение
        config.Place?.RemoveEntity(this);
        newPlace.AddEntity(this);
        config.Place = newPlace;
    }

    protected void Update()
    {
        config.Hunger -= 1;

        if (config.Hunger <= AnimalConfig.LowHungerThreshold1) config.Productivity -= AnimalConfig.ProductivityPenalty1;
        if (config.Hunger <= AnimalConfig.LowHungerThreshold2) config.Productivity -= AnimalConfig.ProductivityPenalty2;
        if (config.Hunger <= AnimalConfig.LowHungerThreshold3) config.Productivity -= AnimalConfig.ProductivityPenalty3;

        if (config.Hunger == 0)
        {
            config.Health = config.MinHealth;
            config.Productivity = config.MinProductivity;
        }

        config.Productivity = CalculateProductivityByAgeAndHealth();

        //TODO Добавить кастомное исключение
        config.Product?.Produce(config.Productivity);
    }

    protected void Eat(int amount)
    {
        config.Hunger += Math.Min(amount, config.MaxFoodIntake);
        Console.WriteLine($"{config.Name} поел(а)");
    }

    //TODO добавить кастомное исключение
    protected void GoToToilet()
    {
        config.Place?.IncreaseDirtiness(config.DirtinessPerToilet);
    }

    private int CalculateProductivityByAgeAndHealth()
    {
        var prod = config.Age <= config.YoungAgeLimit ? config.ProductivityYoung :
            config.Age <= config.AdultAgeLimit ? config.MaxProductivity :
            config.Age <= config.OldAgeLimit ? config.ProductivityMiddle : config.ProductivityOld;

        return Math.Clamp(prod - (config.MaxHealth - config.Health) / 2, config.MinProductivity,
            config.MaxProductivity);
    }
}