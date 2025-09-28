using Farm.Interface;
using Farm.Products;

namespace Farm.Animal;

public abstract class Animal(AnimalConfig config)
    : IPlaceable
{
    protected void Update()
    {
        config.Hunger -= 1;

        if (config.Hunger <= config.LowHungerThreshold1) config.Productivity -= config.ProductivityPenalty1;
        if (config.Hunger <= config.LowHungerThreshold2) config.Productivity -= config.ProductivityPenalty2;
        if (config.Hunger <= config.LowHungerThreshold3) config.Productivity -= config.ProductivityPenalty3;

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
    }

    //TODO добавить кастомное исключение
    public void GoToToilet() => config.Place?.IncreaseDirtiness(config.DirtinessPerToilet);
    public void MakeSound() => Console.WriteLine(config.Sound);

    private int CalculateProductivityByAgeAndHealth()
    {
        int prod = config.Age <= config.YoungAgeLimit ? config.ProductivityYoung :
            config.Age <= config.AdultAgeLimit ? config.MaxProductivity :
            config.Age <= config.OldAgeLimit ? config.ProductivityMiddle : config.ProductivityOld;

        return Math.Clamp(prod - (config.MaxHealth - config.Health) / 2, config.MinProductivity,
            config.MaxProductivity);
    }

    // TODO Разобраться с PLACEPLACE
    public void MoveTo(Place.Place? newPlace)
    {
        if (newPlace == null || config.Place == newPlace) return;
        //TODO добавить кастомное исключение
        config.Place?.RemoveEntity(this);
        newPlace.AddEntity(this);
        config.Place = newPlace;
    }
}