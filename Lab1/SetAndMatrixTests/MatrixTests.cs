namespace SetAndMatrixTests;
using SetAndMatrix.Models.Matrix;
using System.IO;
using SetAndMatrix.Services.Matrix;

public class MatrixTests
{
    private const string Path = "D:\\ConsoleProjects\\SetAndMatrix\\SetAndMatrixTests\\MatrixData\\";

    [Fact]
    public void LoadFromFile_ReturnsMatrix_Valid2x2()
    {
        string filePath = System.IO.Path.Combine(Path, "2x2.txt");
        Matrix fromFile = Matrix.LoadFromFile(filePath);

        Matrix expected = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Assert.Equal(expected,fromFile);
    }

    [Fact]
    public void LoadFromFile_ReturnsMatrix_Valid3x1()
    {
        string filePath = System.IO.Path.Combine(Path, "3x1.txt");
        Matrix fromFile = Matrix.LoadFromFile(filePath);

        Matrix expected = new Matrix(3, 1)
        {
            [0, 0] = 1,
            [1, 0] = 2,
            [2, 0] = 3
        };

        Assert.True(fromFile == expected);
    }

    [Fact]
    public void LoadFromFile_ReturnsMatrix_Valid1x3()
    {
        string filePath = System.IO.Path.Combine(Path, "1x3.txt");
        Matrix fromFile = Matrix.LoadFromFile(filePath);

        Matrix expected = new Matrix(1, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3
        };

        Assert.True(fromFile == expected);
    }

    [Fact]
    public void LoadFromFile_ThrowsInvalidOperation_EmptyFile()
    {
        string filePath = System.IO.Path.Combine(Path, "empty.txt");
        Assert.Throws<InvalidOperationException>(() => Matrix.LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsInvalidOperation_BadFormat()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_format.txt");
        Assert.Throws<InvalidOperationException>(() => Matrix.LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsInvalidOperation_NonNumeric()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_numeric.txt");
        Assert.Throws<InvalidOperationException>(() => Matrix.LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsFileNotFound_NotFound()
    {
        string filePath = System.IO.Path.Combine(Path, "file_not_found.txt");
        Assert.Throws<FileNotFoundException>(() => Matrix.LoadFromFile(filePath));
    }



    [Fact]
    public void Indexer_SetThrowsOutOfRange_InvalidIndices()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[-1, 0] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[2, 0] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[0, -1] = 1.0; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[0, 2] = 1.0; });
    }

    [Fact]
    public void Indexer_GetReturnsValue_ValidIndices()
    {
        Matrix matrix = new Matrix(2, 2)
        {
            [1, 1] = 5.5
        };
        Assert.Equal(5.5, matrix[1, 1]);
    }

    [Fact]
    public void Indexer_SetUpdatesValue_ValidIndices()
    {
        Matrix matrix = new Matrix(2, 2)
        {
            [0, 0] = 10.0
        };
        Assert.Equal(10.0, matrix[0, 0]);
    }

    [Fact]
    public void Expand_IncreasesDimensions_LargerSize()
    {
        Matrix original = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        Matrix expanded = original.Expand(3, 3);

        Assert.Equal(3, expanded.Rows);
        Assert.Equal(3, expanded.Columns);
        Assert.Equal(1, expanded[0, 0]);
        Assert.Equal(4, expanded[1, 1]);
        Assert.Equal(0, expanded[2, 2]);
    }

    [Fact]
    public void Expand_ReturnsSameMatrix_IdenticalSize()
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
    public void Expand_ThrowsArgument_SmallerRows()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(1, 2));
    }

    [Fact]
    public void Expand_ThrowsArgument_SmallerCols()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(2, 1));
    }

    [Fact]
    public void Expand_ThrowsArgument_BothDimensionsSmaller()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Expand(1, 1));
    }

    [Fact]
    public void Cut_ReturnsSmallerMatrix_SmallerSize()
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

        Assert.Equal(2, cut.Rows);
        Assert.Equal(2, cut.Columns);
        Assert.True(cut == expected);
    }

    [Fact]
    public void Cut_ReturnsSameMatrix_IdenticalSize()
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
    public void Cut_ThrowsArgument_LargerRows()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(3, 2));
    }

    [Fact]
    public void Cut_ThrowsArgument_LargerCols()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(2, 3));
    }

    [Fact]
    public void Cut_ThrowsArgument_BothDimensionsLarger()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Cut(3, 3));
    }

    [Fact]
    public void Transpose_SwapsRowsAndColumns()
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

        Assert.Equal(3, transposed.Rows);
        Assert.Equal(2, transposed.Columns);
        Assert.True(transposed == expected);
    }

    [Fact]
    public void Transpose_ReturnsOriginal_CalledTwice()
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
    public void Transpose_HandlesOneByOne()
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
    public void Submatrix_ReturnsCorrect_ValidDimensions()
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
    public void Submatrix_ReturnsOriginal_SameDimensions()
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
    public void Submatrix_ThrowsArgument_LargerHeight()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Submatrix(3, 2));
    }

    [Fact]
    public void Submatrix_ThrowsArgument_LargerWidth()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.Throws<ArgumentException>(() => matrix.Submatrix(2, 3));
    }

    [Fact]
    public void EqualityOperator_ReturnsTrue_EqualMatrices()
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
    public void EqualityOperator_ReturnsFalse_DifferentDimensions()
    {
        Matrix matrixA = new Matrix(2, 2);
        Matrix matrixB = new Matrix(2, 3);

        Assert.False(matrixA == matrixB);
    }

    [Fact]
    public void EqualityOperator_ReturnsFalse_DifferentValues()
    {
        Matrix matrixA = new Matrix(2, 2)
        {
            [0, 0] = 1
        };

        Matrix matrixB = new Matrix(2, 2)
        {
            [0, 0] = 5
        };

        Assert.False(matrixA == matrixB);
    }

    [Fact]
    public void InequalityOperator_ReturnsTrue_NotEqualMatrices()
    {
        var matrixA = new Matrix(2, 2);
        var matrixB = new Matrix(2, 3);

        Assert.True(matrixA != matrixB);
    }

    [Fact]
    public void InequalityOperator_ReturnsFalse_EqualMatrices()
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

        Assert.False(matrixA != matrixB);
    }

    [Fact]
    public void ToString_ReturnsCorrectString()
    {
        Matrix matrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };

        string expected = "1 2 \r\n3 4 \r\n";
        Assert.Equal(expected, matrix.ToString());
    }

    [Fact]
    public void ToString_HandlesSingleElementMatrix()
    {
        Matrix matrix = new Matrix(1, 1)
        {
            [0, 0] = 7
        };
        string expected = "7 \r\n"; 
        Assert.Equal(expected, matrix.ToString());
    }

    [Fact]
    public void ToString_HandlesEmptyMatrix()
    {
        Matrix matrix = new Matrix(0, 0);
        string expected = "";
        Assert.Equal(expected, matrix.ToString());
    }

    [Fact]
    public void Equals_ReturnsTrue_EqualMatrices()
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

        Assert.True(matrixA.Equals(matrixB));
    }

    [Fact]
    public void Equals_ReturnsFalse_NotEqualMatrices()
    {
        Matrix matrixA = new Matrix(2, 2)
        {
            [0, 0] = 1
        };
        Matrix matrixB = new Matrix(2, 2)
        {
            [0, 0] = 5
        };

        Assert.False(matrixA.Equals(matrixB));
    }

    [Fact]
    public void Equals_ReturnsFalse_ComparedWithNull()
    {
        Matrix matrix = new Matrix(2, 2);
        Assert.False(matrix.Equals(null));
    }

    [Fact]
    public void GetHashCode_ReturnsSame_EqualMatrices()
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

        Assert.Equal(matrixA.GetHashCode(), matrixB.GetHashCode());
    }

    [Fact]
    public void IsSquare_ReturnsTrue_SquareMatrix()
    {
        Matrix squareMatrix = new Matrix(2, 2);
        Assert.True(MatrixAnalyzer.IsSquare(squareMatrix));
    }

    [Fact]
    public void IsSquare_ReturnsFalse_NonSquareMatrix()
    {
        Matrix nonSquareMatrix = new Matrix(2, 3);
        Assert.False(MatrixAnalyzer.IsSquare(nonSquareMatrix));
    }

    [Fact]
    public void IsDiagonal_ReturnsTrue_DiagonalMatrix()
    {
        Matrix diagonalMatrix = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [1, 1] = 2,
            [2, 2] = 3
        };
        Assert.True(MatrixAnalyzer.IsDiagonal(diagonalMatrix));
    }

    [Fact]
    public void IsDiagonal_ReturnsFalse_NonDiagonalMatrix()
    {
        Matrix nonDiagonalMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 0,
            [1, 1] = 3
        };
        Assert.False(MatrixAnalyzer.IsDiagonal(nonDiagonalMatrix));
    }

    [Fact]
    public void IsZero_ReturnsTrue_ZeroMatrix()
    {
        Matrix zeroMatrix = new Matrix(2, 2);
        Assert.True(MatrixAnalyzer.IsZero(zeroMatrix));
    }

    [Fact]
    public void IsZero_ReturnsFalse_NonZeroMatrix()
    {
        Matrix nonZeroMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1
        };
        Assert.False(MatrixAnalyzer.IsZero(nonZeroMatrix));
    }

    [Fact]
    public void IsIdentity_ReturnsTrue_IdentityMatrix()
    {
        Matrix identityMatrix = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [1, 1] = 1,
            [2, 2] = 1
        };
        Assert.True(MatrixAnalyzer.IsIdentity(identityMatrix));
    }

    [Fact]
    public void IsIdentity_ReturnsFalse_NonIdentityMatrix()
    {
        Matrix nonIdentityMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 1,
            [1, 0] = 0,
            [1, 1] = 1
        };
        Assert.False(MatrixAnalyzer.IsIdentity(nonIdentityMatrix));
    }

    [Fact]
    public void IsIdentity_ReturnsFalse_NonSquareMatrix()
    {
        Matrix nonSquareMatrix = new Matrix(2, 3);
        Assert.False(MatrixAnalyzer.IsIdentity(nonSquareMatrix));
    }

    [Fact]
    public void IsSymmetric_ReturnsTrue_SymmetricMatrix()
    {
        Matrix symmetricMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 2,
            [1, 1] = 3
        };
        Assert.True(MatrixAnalyzer.IsSymmetric(symmetricMatrix));
    }

    [Fact]
    public void IsSymmetric_ReturnsFalse_NonSymmetricMatrix()
    {
        Matrix nonSymmetricMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [1, 0] = 3,
            [1, 1] = 4
        };
        Assert.False(MatrixAnalyzer.IsSymmetric(nonSymmetricMatrix));
    }

    [Fact]
    public void IsUpperTriangular_ReturnsTrue_UpperTriangular()
    {
        Matrix upperTriangularMatrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 2, [0, 2] = 3,
            [1, 0] = 0, [1, 1] = 4, [1, 2] = 5,
            [2, 0] = 0, [2, 1] = 0, [2, 2] = 6
        };
        Assert.True(MatrixAnalyzer.IsUpperTriangular(upperTriangularMatrix));
    }

    [Fact]
    public void IsUpperTriangular_ReturnsFalse_NonUpperTriangular()
    {
        Matrix nonUpperTriangularMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1, [0, 1] = 2,
            [1, 0] = 3, [1, 1] = 4
        };
        Assert.False(MatrixAnalyzer.IsUpperTriangular(nonUpperTriangularMatrix));
    }

    [Fact]
    public void IsUpperTriangular_ThrowsInvalidOperation_NonSquare()
    {
        Matrix nonSquareMatrix = new Matrix(2, 3);
        Assert.Throws<InvalidOperationException>(() => MatrixAnalyzer.IsUpperTriangular(nonSquareMatrix));
    }

    [Fact]
    public void IsLowerTriangular_ReturnsTrue_LowerTriangular()
    {
        Matrix lowerTriangularMatrix = new Matrix(3, 3)
        {
            [0, 0] = 1, [0, 1] = 0, [0, 2] = 0,
            [1, 0] = 2, [1, 1] = 3, [1, 2] = 0,
            [2, 0] = 4, [2, 1] = 5, [2, 2] = 6
        };
        Assert.True(MatrixAnalyzer.IsLowerTriangular(lowerTriangularMatrix));
    }

    [Fact]
    public void IsLowerTriangular_ReturnsFalse_NonLowerTriangular()
    {
        Matrix nonLowerTriangularMatrix = new Matrix(2, 2)
        {
            [0, 0] = 1, [0, 1] = 2,
            [1, 0] = 3, [1, 1] = 4
        };
        Assert.False(MatrixAnalyzer.IsLowerTriangular(nonLowerTriangularMatrix));
    }

    [Fact]
    public void IsLowerTriangular_ThrowsInvalidOperation_NonSquare()
    {
        Matrix nonSquareMatrix = new Matrix(2, 3);
        Assert.Throws<InvalidOperationException>(() => MatrixAnalyzer.IsLowerTriangular(nonSquareMatrix));
    }
}
