namespace SetAndMatrix.Interfaces.Multiset;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для валидации мультимножеств.</brief>
/// <details>Этот интерфейс определяет статический метод для валидации строкового представления мультимножества.</details>
/// </file>
public interface IMultisetValidator
{
    /// <summary>
    /// Выполняет валидацию строкового представления мультимножества.
    /// </summary>
    /// <param name="input">Строковое представление мультимножества для валидации.</param>
    /// <exception cref="System.InvalidOperationException">Выбрасывается, если строка пуста, содержит несбалансированные скобки, запятые вне множества или  запятые подряд.</exception>
    static abstract void Validate(string input);
} 