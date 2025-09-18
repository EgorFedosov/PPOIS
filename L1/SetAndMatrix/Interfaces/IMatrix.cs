namespace SetAndMatrix.Interfaces;

using Models;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для работы с матрицей.</brief>
/// <details>Определяет методы для анализа, трансформации и валидации матриц.</details>
/// </file>
public interface IMatrix
{
    /// <summary>
    /// Определяет, является ли матрица квадратной.
    /// </summary>
    /// <returns>True, если матрица квадратная; иначе — false.</returns>
    bool IsSquare();

    /// <summary>
    /// Определяет, является ли матрица диагональной.
    /// </summary>
    /// <returns>True, если матрица диагональная; иначе — false.</returns>
    bool IsDiagonal();

    /// <summary>
    /// Определяет, является ли матрица нулевой.
    /// </summary>
    /// <returns>True, если матрица нулевая; иначе — false.</returns>
    bool IsZero();

    /// <summary>
    /// Определяет, является ли матрица единичной.
    /// </summary>
    /// <returns>True, если матрица единичная; иначе — false.</returns>
    bool IsIdentity();

    /// <summary>
    /// Определяет, является ли матрица симметрической.
    /// </summary>
    /// <returns>True, если матрица симметрическая; иначе — false.</returns>
    bool IsSymmetric();

    /// <summary>
    /// Определяет, является ли матрица верхней треугольной.
    /// </summary>
    /// <returns>True, если матрица верхняя треугольная; иначе — false.</returns>
    bool IsUpperTriangular();

    /// <summary>
    /// Определяет, является ли матрица нижней треугольной.
    /// </summary>
    /// <returns>True, если матрица нижняя треугольная; иначе — false.</returns>
    bool IsLowerTriangular();

    /// <summary>
    /// Создаёт и возвращает транспонированную матрицу.
    /// </summary>
    /// <returns>Новая матрица, транспонированная относительно текущей.</returns>
    Matrix Transpose();

    /// <summary>
    /// Возвращает подматрицу указанного размера.
    /// </summary>
    /// <param name="h">Количество строк подматрицы.</param>
    /// <param name="w">Количество столбцов подматрицы.</param>
    /// <returns>Новая подматрица размера h×w.</returns>
    Matrix Submatrix(int h, int w);

    /// <summary>
    /// Расширяет матрицу до новых размеров.
    /// </summary>
    /// <param name="newRows">Новое количество строк.</param>
    /// <param name="newCols">Новое количество столбцов.</param>
    /// <returns>Новая расширенная матрица.</returns>
    Matrix Expand(int newRows, int newCols);

    /// <summary>
    /// Обрезает матрицу до новых размеров.
    /// </summary>
    /// <param name="newRows">Новое количество строк.</param>
    /// <param name="newCols">Новое количество столбцов.</param>
    /// <returns>Новая обрезанная матрица.</returns>
    Matrix Cut(int newRows, int newCols);

    /// <summary>
    /// Загружает матрицу из файла.
    /// </summary>
    /// <param name="path">Путь к файлу с данными матрицы.</param>
    /// <returns>Новая матрица, считанная из файла.</returns>
    Matrix LoadFromFile(string path);
}