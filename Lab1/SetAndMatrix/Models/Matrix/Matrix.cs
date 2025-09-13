namespace SetAndMatrix.Models.Matrix;

using System.Text;
using Services.Matrix;
using Services;

public class Matrix(int rows, int columns)
{
    private readonly double[,] _data = new double[rows, columns];
    public int Rows => _data.GetLength(0);
    public int Columns => _data.GetLength(1);

    public double this[int row, int col]
    {
        get
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                throw new ArgumentOutOfRangeException();
            return _data[row, col];
        }
        set
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                throw new ArgumentOutOfRangeException();
            _data[row, col] = value;
        }
    }

    public Matrix(Matrix other) : this(other.Rows, other.Columns)
    {
        Array.Copy(other._data, _data, other._data.Length);
    }

    public static Matrix LoadFromFile(string path)
    {
        MatrixValidator.ValidateFile(path);

        var lines = File.ReadAllLines(path);
        int rows = lines.Length;
        int cols = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        var matrix = new Matrix(rows, cols);

        for (int i = 0; i < rows; i++)
        {
            var values = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = double.Parse(values[j]);
            }
        }

        return matrix;
    }

    public Matrix Expand(int newRows, int newCols)
    {
        if (newRows < Rows || newCols < Columns)
            throw new ArgumentException("Новые размеры должны быть больше или равны текущим.");

        var expanded = new Matrix(newRows, newCols);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                expanded[i, j] = _data[i, j];
            }
        }

        return expanded;
    }

    public Matrix Cut(int newRows, int newCols)
    {
        if (newRows > Rows || newCols > Columns)
            throw new ArgumentException("Новые размеры должны быть меньше или равны текущим.");

        var cut = new Matrix(newRows, newCols);

        for (int i = 0; i < newRows; i++)
        {
            for (int j = 0; j < newCols; j++)
            {
                cut[i, j] = _data[i, j];
            }
        }

        return cut;
    }

    public Matrix Transpose()
    {
        var result = new Matrix(Columns, Rows);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result[j, i] = _data[i, j];
            }
        }

        return result;
    }

    public Matrix Submatrix(int h, int w)
    {
        if (h > Rows || w > Columns)
            throw new ArgumentException("Размер подматрицы превышает размеры исходной");

        var result = new Matrix(h, w);
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                result[i, j] = this[i, j];
            }
        }

        return result;
    }

    public static bool operator ==(Matrix? a, Matrix? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        if (a.Rows != b.Rows || a.Columns != b.Columns) return false;

        for (int i = 0; i < a.Rows; i++)
        for (int j = 0; j < a.Columns; j++)
            if (Math.Abs(a[i, j] - b[i, j]) > Constants.Tolerance)
                return false;

        return true;
    }

    public static bool operator !=(Matrix a, Matrix b) => !(a == b);

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
                sb.Append(_data[i, j] + " ");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Matrix other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        int hash = Constants.HashSeed;
        for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
            hash = hash * Constants.HashMultiplier + _data[i, j].GetHashCode();
        return hash;
    }
}