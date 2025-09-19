namespace SetAndMatrixTests;

using SetAndMatrix.Models;
using System.IO;

public class MatrixTests
{
    private const string Path = "D:\\ConsoleProjects\\SetAndMatrix\\SetAndMatrixTests\\MatrixData\\";

    [Fact]
    public void LoadFromFile_2x2_Valid()
    {
        string filePath = System.IO.Path.Combine(Path, "2x2.txt");
        Matrix fromFile = new Matrix(2, 2).LoadFromFile(filePath);

        Matrix expected = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Assert.True(fromFile == expected);
    }

    [Fact]
    public void LoadFromFile_3x1_Valid()
    {
        string filePath = System.IO.Path.Combine(Path, "3x1.txt");
        Matrix fromFile = new Matrix(3, 1).LoadFromFile(filePath);

        Matrix expected = new Matrix(3, 1)
        {
            [0, 0] = 1,
            [1, 0] = 2,
            [2, 0] = 3
        };

        Assert.True(fromFile == expected);
    }

    [Fact]
    public void LoadFromFile_1x3_Valid()
    {
        string filePath = System.IO.Path.Combine(Path, "1x3.txt");
        Matrix fromFile = new Matrix(1, 3).LoadFromFile(filePath);

        Matrix expected = new Matrix(1, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3
        };

        Assert.True(fromFile == expected);
    }

    [Fact]
    public void LoadFromFile_Empty_Throws()
    {
        string filePath = System.IO.Path.Combine(Path, "empty.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_BadFormat_Throws()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_format.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_NonNumeric_Throws()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_numeric.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_NotFound_Throws()
    {
        string filePath = System.IO.Path.Combine(Path, "file_not_found.txt");
        Assert.Throws<FileNotFoundException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void Indexer_Set_InvalidIndices_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[-1, 0] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[2, 0] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[0, -1] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[0, 2] = 1.0; });
    }

    [Fact]
    public void Indexer_Get_ValidIndices_Works()
    {
        Matrix matrix = new Matrix(2, 2)
        {
            [1, 1] = 5.5
        };
        Assert.Equal(5.5, matrix[1, 1]);
    }

    [Fact]
    public void Indexer_Set_ValidIndices_Works()
    {
        Matrix matrix = new Matrix(2, 2)
        {
            [0, 0] = 10.0
        };
        Assert.Equal(10.0, matrix[0, 0]);
    }

    [Fact]
    public void Expand_SameSize_ReturnsSame()
    {
        Matrix original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix expanded = original.Expand(2, 2);

        Assert.True(original == expanded);
    }

    [Fact]
    public void Expand_SmallerRows_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(1, 2));
    }

    [Fact]
    public void Expand_SmallerCols_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(2, 1));
    }

    [Fact]
    public void Expand_SmallerBoth_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(1, 1));
    }

    [Fact]
    public void Cut_SmallerSize_Works()
    {
        Matrix original = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [1, 0] = 4,
            [1, 1] = 5,
            [1, 2] = 6,
            [2, 0] = 7,
            [2, 1] = 8,
            [2, 2] = 9
        };

        Matrix cut = original.Cut(2, 2);

        Matrix expected = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 4,
            [1, 1] = 5
        };

        Assert.True(cut == expected);
    }

    [Fact]
    public void Cut_SameSize_ReturnsSame()
    {
        Matrix original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix cut = original.Cut(2, 2);

        Assert.True(original == cut);
    }

    [Fact]
    public void Cut_LargerRows_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(3, 2));
    }

    [Fact]
    public void Cut_LargerCols_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(2, 3));
    }

    [Fact]
    public void Cut_LargerBoth_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(3, 3));
    }

    [Fact]
    public void Transpose_SwapsDimensions()
    {
        Matrix original = new Matrix(2, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [1, 0] = 4,
            [1, 1] = 5,
            [1, 2] = 6
        };

        Matrix transposed = original.Transpose();

        Matrix expected = new Matrix(3, 2)
        {
            [0, 0] = 1,
            [0, 1] = 4,
            [1, 0] = 2,
            [1, 1] = 5,
            [2, 0] = 3,
            [2, 1] = 6
        };

        Assert.True(transposed == expected);
    }

    [Fact]
    public void Transpose_Twice_ReturnsOriginal()
    {
        Matrix original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix transposedTwice = original.Transpose().Transpose();

        Assert.True(original == transposedTwice);
    }

    [Fact]
    public void Transpose_1x1_Works()
    {
        Matrix original = new Matrix(1, 1)
        {
            [0, 0] = 7
        };

        Matrix transposed = original.Transpose();

        Matrix expected = new Matrix(1, 1)
        {
            [0, 0] = 7
        };

        Assert.True(transposed == expected);
    }

    [Fact]
    public void Submatrix_ValidDimensions_Works()
    {
        Matrix original = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [1, 0] = 4,
            [1, 1] = 5,
            [1, 2] = 6,
            [2, 0] = 7,
            [2, 1] = 8,
            [2, 2] = 9
        };

        Matrix submatrix = original.Submatrix(2, 2);

        Matrix expected = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 4,
            [1, 1] = 5
        };

        Assert.True(submatrix == expected);
    }

    [Fact]
    public void Submatrix_SameDimensions_ReturnsSame()
    {
        Matrix original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix submatrix = original.Submatrix(2, 2);

        Assert.True(submatrix == original);
    }

    [Fact]
    public void Submatrix_LargerHeight_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Submatrix(3, 2));
    }

    [Fact]
    public void Submatrix_LargerWidth_Throws()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Submatrix(2, 3));
    }

    [Fact]
    public void Equality_EqualMatrices_True()
    {
        Matrix matrixA = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix matrixB = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Assert.True(matrixA == matrixB);
    }

    [Fact]
    public void Equality_DifferentDimensions_False()
    {
        Matrix matrixA = new Matrix(2, 2);
        Matrix matrixB = new Matrix(2, 3);

        Assert.False(matrixA == matrixB);
    }
    [Fact]
    public void Constructor_WithDimensions_CreatesZeroMatrix()
    {
        var matrix = new Matrix(2, 3);
        Assert.True(matrix.IsZero());
    }

    [Fact]
    public void CopyConstructor_CreatesIdenticalMatrix()
    {
        var original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        var copy = new Matrix(original);
        Assert.True(original == copy);
    }

    [Fact]
    public void IsSquare_SquareMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3);
        Assert.True(matrix.IsSquare());
    }

    [Fact]
    public void IsSquare_NonSquareMatrix_ReturnsFalse()
    {
        var matrix = new Matrix(2, 3);
        Assert.False(matrix.IsSquare());
    }

    [Fact]
    public void IsDiagonal_DiagonalMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [1, 1] = 2,
            [2, 2] = 3
        };
        Assert.True(matrix.IsDiagonal());
    }

    [Fact]
    public void IsDiagonal_NonDiagonalMatrix_ReturnsFalse()
    {
        var matrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 1, // не диагональный элемент
            [1, 0] = 0,
            [1, 1] = 2
        };
        Assert.False(matrix.IsDiagonal());
    }

    [Fact]
    public void IsZero_ZeroMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(2, 2);
        Assert.True(matrix.IsZero());
    }

    [Fact]
    public void IsZero_NonZeroMatrix_ReturnsFalse()
    {
        var matrix = new Matrix(2, 2)
        {
            [0, 0] = 1
        };
        Assert.False(matrix.IsZero());
    }

    [Fact]
    public void IsIdentity_IdentityMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [1, 1] = 1,
            [2, 2] = 1
        };
        Assert.True(matrix.IsIdentity());
    }

    [Fact]
    public void IsIdentity_NonIdentityMatrix_ReturnsFalse()
    {
        var matrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 0,
            [1, 0] = 0,
            [1, 1] = 2 // должно быть 1
        };
        Assert.False(matrix.IsIdentity());
    }

    [Fact]
    public void IsSymmetric_SymmetricMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 2, [0, 2] = 3,
            [1, 0] = 2, [1, 1] = 4, [1, 2] = 5,
            [2, 0] = 3, [2, 1] = 5, [2, 2] = 6
        };
        Assert.True(matrix.IsSymmetric());
    }

    [Fact]
    public void IsSymmetric_NonSymmetricMatrix_ReturnsFalse()
    {
        var matrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3, // не равно [0,1]
            [1, 1] = 4
        };
        Assert.False(matrix.IsSymmetric());
    }

    [Fact]
    public void IsUpperTriangular_UpperTriangularMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 2, [0, 2] = 3,
            [1, 0] = 0, [1, 1] = 4, [1, 2] = 5,
            [2, 0] = 0, [2, 1] = 0, [2, 2] = 6
        };
        Assert.True(matrix.IsUpperTriangular());
    }

    [Fact]
    public void IsUpperTriangular_NonUpperTriangular_ReturnsFalse()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 2, [0, 2] = 3,
            [1, 0] = 1, // не ноль
            [1, 1] = 4, [1, 2] = 5,
            [2, 0] = 0, [2, 1] = 0, [2, 2] = 6
        };
        Assert.False(matrix.IsUpperTriangular());
    }

    [Fact]
    public void IsLowerTriangular_LowerTriangularMatrix_ReturnsTrue()
    {
        var matrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 0, [0, 2] = 0,
            [1, 0] = 2, [1, 1] = 3, [1, 2] = 0,
            [2, 0] = 4, [2, 1] = 5, [2, 2] = 6
        };
        Assert.True(matrix.IsLowerTriangular());
    }

    [Fact]
    public void Equals_SameMatrix_ReturnsTrue()
    {
        var matrix1 = new Matrix(2, 2) { [0, 0] = 1, [0, 1] = 2, [1, 0] = 3, [1, 1] = 4 };
        var matrix2 = new Matrix(2, 2) { [0, 0] = 1, [0, 1] = 2, [1, 0] = 3, [1, 1] = 4 };
        
        Assert.True(matrix1.Equals(matrix2));
    }

    [Fact]
    public void Equals_DifferentMatrix_ReturnsFalse()
    {
        var matrix1 = new Matrix(2, 2) { [0, 0] = 1 };
        var matrix2 = new Matrix(2, 2) { [0, 0] = 2 };
        
        Assert.False(matrix1.Equals(matrix2));
    }

    [Fact]
    public void Equals_NonMatrixObject_ReturnsFalse()
    {
        var matrix = new Matrix(2, 2);
        var otherObject = new object();
        
        Assert.False(matrix.Equals(otherObject));
    }

    [Fact]
    public void GetHashCode_EqualMatrices_HaveSameHashCode()
    {
        var matrix1 = new Matrix(2, 2) { [0, 0] = 1, [0, 1] = 2, [1, 0] = 3, [1, 1] = 4 };
        var matrix2 = new Matrix(2, 2) { [0, 0] = 1, [0, 1] = 2, [1, 0] = 3, [1, 1] = 4 };
        
        Assert.Equal(matrix1.GetHashCode(), matrix2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var matrix = new Matrix(2, 2)
        {
            [0, 0] = 1.5,
            [0, 1] = 2.3,
            [1, 0] = 3.7,
            [1, 1] = 4.1
        };
        
        var result = matrix.ToString();
        Assert.Contains("1,5", result);
        Assert.Contains("2,3", result);
        Assert.Contains("3,7", result);
        Assert.Contains("4,1", result);
    }

    [Fact]
    public void OperatorNotEqual_ReturnsCorrectValue()
    {
        var matrix1 = new Matrix(2, 2) { [0, 0] = 1 };
        var matrix2 = new Matrix(2, 2) { [0, 0] = 2 };
        
        Assert.True(matrix1 != matrix2);
    }

    [Fact]
    public void ValidateFile_EmptyPath_Throws()
    {
        var matrix = new Matrix(1, 1);
        Assert.Throws<ArgumentException>(() => matrix.LoadFromFile(""));
    }

    [Fact]
    public void ValidateFile_NullPath_Throws()
    {
        var matrix = new Matrix(1, 1);
        Assert.Throws<ArgumentException>(() => matrix.LoadFromFile(null));
    }
}