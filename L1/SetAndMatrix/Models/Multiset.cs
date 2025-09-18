namespace SetAndMatrix.Models;

using System.Text;
using Interfaces;
public class Multiset:IMultiset
{
    private readonly List<MultisetElement> _elements = [];
    private void Add(MultisetElement element) => _elements.Add(element);

    public Multiset()
    {
    }

    public Multiset(string set)
    {
        Parse(set);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('{');

        for (int i = 0; i < _elements.Count; i++)
        {
            var el = _elements[i];
            if (el.Element != null)
                sb.Append((string?)el.Element);
            else if (el.Nested != null)
                sb.Append(el.Nested);

            if (i < _elements.Count - 1)
                sb.Append(',');
        }

        sb.Append('}');
        return sb.ToString();
    }

    public Multiset Parse(string set)
    {
        Validate(set);
        var stack = new Stack<Multiset>();
        string element = "";
        Multiset? root = null;

        foreach (var c in set)
        {
            if (c == '{')
            {
                var multiset = new Multiset();

                if (stack.Count > 0)
                    stack.Peek().Add(new MultisetElement(multiset));
                else
                    root = multiset;

                stack.Push(multiset);
            }
            else if (c == '}')
            {
                AddElementIfNotEmpty();
                stack.Pop();
            }
            else if (c == ',')
            {
                AddElementIfNotEmpty();
            }
            else
            {
                if (c == ' ') continue;
                element += c;
            }
        }

        if (!string.IsNullOrWhiteSpace(element) && stack.Count > 0)
            stack.Peek().Add(new MultisetElement(element));

        return root!;

        void AddElementIfNotEmpty()
        {
            if (!string.IsNullOrWhiteSpace(element))
            {
                stack.Peek().Add(new MultisetElement(element));
                element = "";
            }
        }
    }

    /// <summary>
    /// Выполняет валидацию строкового представления мультимножества.
    /// </summary>
    /// <param name="input">Строка для валидации.</param>
    /// <exception cref="System.InvalidOperationException">
    /// Выбрасывается, если строка пуста, содержит несбалансированные скобки, запятые вне множества или запятые подряд.
    /// </exception>
    private void Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidOperationException("Строка пуста.");

        int balance = 0;
        bool lastWasComma = false;

        foreach (char c in input)
        {
            switch (c)
            {
                case '{':
                    balance++;
                    lastWasComma = false;
                    break;
                case '}':
                    balance--;
                    if (balance < 0)
                        throw new InvalidOperationException("Лишняя закрывающая скобка '}'.");
                    if (lastWasComma)
                        throw new InvalidOperationException("Запятая перед '}' недопустима.");
                    lastWasComma = false;
                    break;
                case ',':
                    if (balance == 0)
                        throw new InvalidOperationException("Запятая вне множества.");
                    if (lastWasComma)
                        throw new InvalidOperationException("Запятые подряд недопустимы.");
                    lastWasComma = true;
                    break;
                default:
                    lastWasComma = false;
                    break;
            }
        }

        if (balance != 0)
            throw new InvalidOperationException("Несоответствие скобок.");
        if (lastWasComma)
            throw new InvalidOperationException("Запятая в конце множества недопустима.");
    }
}