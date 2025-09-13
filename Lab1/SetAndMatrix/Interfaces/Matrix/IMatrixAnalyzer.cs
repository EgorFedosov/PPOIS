namespace SetAndMatrix.Interfaces.Matrix;

/// <file>
/// <author>Egor Fedosov</author>
/// <brief>Интерфейс для анализа матриц.</brief>
/// <details>Этот интерфейс определяет статические методы для проверки различных свойств матриц, проверка типа матрицы (квадратная, диагональная, нулевая, единичная, симметрическая, верхняя треугольная, нижняя треугольная).</details>
/// </file>
public interface IMatrixAnalyzer
{
    /// <summary>
    /// Определяет, является ли матрица квадратной.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица квадратная; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsSquare(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица диагональной.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица диагональная; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsDiagonal(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица нулевой.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица нулевая; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsZero(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица единичной.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица единичная; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsIdentity(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица симметрической.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица симметрическая; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsSymmetric(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица верхняя треугольная.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица верхняя треугольная; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsUpperTriangular(Models.Matrix.Matrix matrix);

    /// <summary>
    /// Определяет, является ли матрица нижняя треугольная.
    /// </summary>
    /// <param name="matrix">Матрица для проверки.</param>
    /// <returns>True, если матрица нижняя треугольная; в противном случае — false.</returns>
    /// <seealso cref="SetAndMatrix.Models.Matrix.Matrix"/>
    static abstract bool IsLowerTriangular(Models.Matrix.Matrix matrix);
} 