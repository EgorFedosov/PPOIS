namespace SetAndMatrix.Services.Matrix;

using Interfaces.Matrix;
using Models.Matrix;

public class MatrixAnalyzer : IMatrixAnalyzer
{
    public static bool IsSquare(Matrix matrix)
        => matrix.Columns == matrix.Rows;

    public static bool IsDiagonal(Matrix matrix)
    {
        for (int i = 0; i < matrix.Columns; i++)
        {
            for (int j = 0; j < matrix.Rows; j++)
            {
                if (i != j && matrix[i, j] != 0) return false;
            }
        }

        return true;
    }

    public static bool IsZero(Matrix matrix)
    {
        for (int i = 0; i < matrix.Columns; i++)
        {
            for (int j = 0; j < matrix.Rows; j++)
            {
                if (matrix[i, j] != 0) return false;
            }
        }

        return true;
    }

    public static bool IsIdentity(Matrix matrix)
    {
        if (matrix.Rows != matrix.Columns) return false;

        for (int i = 0; i < matrix.Columns; i++)
        {
            for (int j = 0; j < matrix.Rows; j++)
            {
                if (i == j)
                {
                    if (Math.Abs(matrix[i, j] - 1) > Constants.Tolerance) return false;
                }
                else
                {
                    if (matrix[i, j] != 0) return false;
                }
            }
        }

        return true;
    }


    public static bool IsSymmetric(Matrix matrix)
    {
        for (int i = 0; i < matrix.Columns; i++)
        {
            for (int j = 0; j < matrix.Rows; j++)
            {
                if (Math.Abs(matrix[i, j] - matrix[j, i]) > Constants.Tolerance) return false;
            }
        }

        return true;
    }

    public static bool IsUpperTriangular(Matrix matrix)
    {
        if (!IsSquare(matrix)) throw new InvalidOperationException("Матрица должна быть квадратной.");

        for (int i = 1; i < matrix.Rows; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (matrix[i, j] != 0) return false;
            }
        }

        return true;
    }

    public static bool IsLowerTriangular(Matrix matrix)
    {
        if (!IsSquare(matrix)) throw new InvalidOperationException("Матрица должна быть квадратной.");

        for (int i = 0; i < matrix.Rows; i++)
        {
            for (int j = i + 1; j < matrix.Columns; j++)
            {
                if (matrix[i, j] != 0) return false;
            }
        }

        return true;
    }
}