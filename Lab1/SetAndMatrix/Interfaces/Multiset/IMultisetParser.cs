namespace SetAndMatrix.Interfaces.Multiset;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для парсера мультимножеств.</brief>
/// <details>Этот интерфейс определяет статический метод для парсинга строкового представления мультимножества в объект мультимножества.</details>
/// </file>
public interface IMultisetParser
{
    /// <summary>
    /// Парсит строковое представление мультимножества.
    /// </summary>
    /// <param name="set">Строковое представление мультимножества.</param>
    /// <returns>Объект мультимножества, построенный из строки.</returns>
    /// <seealso cref="SetAndMatrix.Models.Multiset.Multiset"/>
    static abstract Models.Multiset.Multiset Parse(string set);
} 