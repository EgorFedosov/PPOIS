namespace SetAndMatrix.Interfaces;

using Models;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для работы с мультимножеством.</brief>
/// <details>Определяет методы парсинга и валидации строкового представления мультимножества.</details>
/// </file>
public interface IMultiset
{
    /// <summary>
    /// Парсит строковое представление мультимножества и заполняет текущий объект.
    /// </summary>
    /// <param name="set">Строковое представление мультимножества.</param>
    /// <returns>Текущий объект мультимножества, заполненный данными из строки.</returns>
    Multiset Parse(string set);


    /// <summary>
    /// Возвращает строковое представление мультимножества.
    /// </summary>
    /// <returns>Строка в формате {элементы}.</returns>
    string ToString();
}