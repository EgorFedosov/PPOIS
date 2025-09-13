namespace SetAndMatrix.Models.Multiset;

using System.Text;

public class Multiset
{
    private readonly List<MultisetElement> _elements = [];
    public void Add(MultisetElement element) => _elements.Add(element);

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('{');

        for (int i = 0; i < _elements.Count; i++)
        {
            var el = _elements[i];
            if (el.Element != null)
                sb.Append(el.Element);
            else if (el.Nested != null)
                sb.Append(el.Nested);

            if (i < _elements.Count - 1)
                sb.Append(',');
        }

        sb.Append('}');
        return sb.ToString();
    }
}