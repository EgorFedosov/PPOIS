namespace SetAndMatrix.Interfaces.Matrix;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для валидации матриц.</brief>
/// <details>Этот интерфейс определяет статические методы для валидации файлов, содержащих данные матрицы.</details>
/// </file>
public interface IMatrixValidator
{
    /// <summary>
    /// Выполняет валидацию файла, содержащего данные матрицы.
    /// </summary>
    /// <param name="path">Путь к файлу для валидации.</param>
    /// <exception cref="System.IO.FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    /// <exception cref="System.InvalidOperationException">Выбрасывается, если файл пуст, содержит строки с разным количеством элементов или некорректные числовые значения.</exception>
    static abstract void ValidateFile(string path);
} 