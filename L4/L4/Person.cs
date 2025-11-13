namespace L4;

public class Person(string name, int age) : IComparable<Person>
{
    private string Name { get; } = name;
    private int Age { get; } = age;

    public int CompareTo(Person? other)
    {
        return other == null ? 1 : Age.CompareTo(other.Age);
    }

    public override string ToString() => $"{Name} ({Age})";

    public override bool Equals(object? obj)
    {
        if (obj is not Person other) return false;
        return Name == other.Name && Age == other.Age;
    }

    public override int GetHashCode() => HashCode.Combine(Name, Age);
}
