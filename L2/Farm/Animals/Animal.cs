using Farm.Configs;
using Farm.Exceptions;
using Farm.Interfaces;
using Farm.Places;
using Farm.Products;

namespace Farm.Animals;

public abstract class Animal : IAnimal
{
    private readonly AnimalConfig _config;
    private Place? _place;

    protected Animal(AnimalConfig config)
    {
        _config = config;
        Console.WriteLine($"{config.Name} создано.");

        if (config.Place != null)
        {
            Place = config.Place;
        }
    }

    private Place? Place
    {
        get => _place;
        set
        {
            if (_place == value) return;

            _place?.RemoveEntity(this);
            _place = value;
            _config.Place = value;
            _place?.AddEntity(this);
        }
    }

    public Product? Product => _config.Product;

    public void PrintStats()
    {
        Console.WriteLine($"Имя: {_config.Name}");
        Console.WriteLine($"Возраст: {_config.Age}");
        Console.WriteLine($"Здоровье: {_config.Health}");
        Console.WriteLine($"Сытость: {_config.Hunger}");
        Console.WriteLine($"Продуктивность: {_config.Productivity}");
    }

    public void MakeSound() => Console.WriteLine(_config.Sound);

    public void Die()
    {
        if (_config.Health <= 0) throw new AnimalAlreadyDeadException($"{_config.Name} уже мертво");
        
        Console.WriteLine($"{_config.Name} умер(ла).");
        Place = null;
        _config.Health = 0;
    }

    public void MoveTo(Place newPlace)
    {
        if (Place == newPlace)
            throw new InvalidMoveException($"{_config.Name} уже находится на {newPlace.Name}");
        
        Place = newPlace;
    }

    public void Update()
    {
        _config.Hunger -= 1;

        if (_config.Hunger <= AnimalConfig.LowHungerThreshold1) _config.Productivity -= AnimalConfig.ProductivityPenalty1;
        if (_config.Hunger <= AnimalConfig.LowHungerThreshold2) _config.Productivity -= AnimalConfig.ProductivityPenalty2;
        if (_config.Hunger <= AnimalConfig.LowHungerThreshold3) _config.Productivity -= AnimalConfig.ProductivityPenalty3;

        if (_config.Hunger == 0)
        {
            _config.Health = _config.MinHealth;
            _config.Productivity = _config.MinProductivity;
        }

        _config.Productivity = CalculateProductivityByAgeAndHealth();
        if (_config.Productivity == 0)
            throw new AnimalMissingProductException("У животного не инициализировано поле Product");
        
        _config.Product?.Produce(_config.Productivity);
    }

    public void Eat(int amount)
    {
        _config.Hunger += Math.Min(amount, _config.MaxFoodIntake);
        Console.WriteLine($"{_config.Name} поел(а)");
    }

    protected void GoToToilet()
    {
        if (Place == null)
            throw new AnimalInvalidPlaceStateException("Животное не может загрязнять территорию, если его местоположение не задано");
        
        Place.IncreaseDirtiness(_config.DirtinessPerToilet);
    }

    private int CalculateProductivityByAgeAndHealth()
    {
        var prod = _config.Age <= _config.YoungAgeLimit ? _config.ProductivityYoung :
            _config.Age <= _config.AdultAgeLimit ? _config.MaxProductivity :
            _config.Age <= _config.OldAgeLimit ? _config.ProductivityMiddle : _config.ProductivityOld;

        return Math.Clamp(prod - (_config.MaxHealth - _config.Health) / 2, _config.MinProductivity, _config.MaxProductivity);
    }
}