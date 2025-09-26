using Farm.Products;
using Farm.Interface;

namespace Farm.Animal;
using Place;

public abstract class Animal(string name, int age, Place place, Product product, AnimalConfig config)
    : IPlaceable
{
    private int _health;
    private int _productivity;
    private int _hunger;

    protected string Name { get; } = name;
    protected int Age { get; } = age;
    protected Place Place { get; private set; } = place;
    protected Product Product { get; } = product;

    private string Sound => config.Sound;
    private int MaxFoodIntake => config.MaxFoodIntake;
    private float DirtinessPerToilet => config.DirtinessPerToilet;

    private int Hunger
    {
        get => _hunger;
        set => _hunger = Math.Clamp(value, config.MinHungry, config.MaxHungry);
    }

    protected int Productivity
    {
        get => _productivity;
        set => _productivity = Math.Clamp(value, config.MinProductivity, config.MaxProductivity);
    }

    protected int Health
    {
        get => _health;
        private set => _health = Math.Clamp(value, config.MinHealth, config.MaxHealth);
    }

    protected virtual void Update()
    {
        Hunger -= 1;
        if (Hunger <= 30) Productivity -= 1;
        if (Hunger <= 20) Productivity -= 3;
        if (Hunger <= 10) Productivity -= 7;

        if (Hunger == 0)
        {
            Health = config.MinHealth;
            Productivity = config.MinProductivity;
        }

        Productivity = CalculateProductivityByAgeAndHealth(Age, Health,
            config.YoungAgeLimit, config.AdultAgeLimit, config.OldAgeLimit);

        Product.Produce(Productivity);
    }

    protected virtual void Eat(int amount)
    {
        Hunger += Math.Min(amount, MaxFoodIntake);
    }

    public virtual void GoToToilet() => Place.IncreaseDirtiness(DirtinessPerToilet);
    public virtual void MakeSound() => Console.WriteLine(Sound);

    protected virtual int CalculateProductivityByAgeAndHealth(int age, int health, int youngAgeLimit, int adultAgeLimit, int oldAgeLimit)
    {
        int prod = age <= youngAgeLimit ? 30 :
                   age <= adultAgeLimit ? config.MaxProductivity :
                   age <= oldAgeLimit ? 70 : 20;

        return Math.Clamp(prod - (config.MaxHealth - health) / 2, config.MinProductivity, config.MaxProductivity);
    }
    public void MoveTo(Place? newPlace)
    {
        if (newPlace == null || Place == newPlace) return;

        Place.RemoveEntity(this);
        newPlace.AddEntity(this);
        Place = newPlace;
    }
}
