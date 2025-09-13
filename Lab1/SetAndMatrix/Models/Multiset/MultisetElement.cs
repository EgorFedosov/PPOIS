namespace SetAndMatrix.Models.Multiset;

public class MultisetElement
{
    public string? Element { get; }
    public Multiset? Nested { get; }

    public MultisetElement(string element)
    {
        Element = element;
    }

    public MultisetElement(Multiset nested)
    {
        Nested = nested;
    }
}