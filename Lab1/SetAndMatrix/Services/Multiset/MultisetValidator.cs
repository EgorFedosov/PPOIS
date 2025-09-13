namespace SetAndMatrix.Services.Multiset;

using Interfaces.Multiset;

public class MultisetValidator : IMultisetValidator
{
    public static void Validate(string input)
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