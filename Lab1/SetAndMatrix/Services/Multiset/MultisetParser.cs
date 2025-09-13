namespace SetAndMatrix.Services.Multiset;
using Models.Multiset;
using Interfaces.Multiset;

public class MultisetParser:IMultisetParser
{
    public static Multiset Parse(string set)
    {
        MultisetValidator.Validate(set);

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
}