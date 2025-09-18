namespace SetAndMatrixTests;

using SetAndMatrix.Models;
using System.IO;

public class MatrixTests
{
    private const string Path = "D:\\ConsoleProjects\\SetAndMatrix\\SetAndMatrixTests\\MatrixData\\";

    [Fact]
    public void LoadFromFile_ReturnsMatrix_Valid2x2()
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
    public void LoadFromFile_ReturnsMatrix_Valid3x1()
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
    public void LoadFromFile_ReturnsMatrix_Valid1x3()
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
    public void LoadFromFile_ThrowsInvalidOperation_EmptyFile()
    {
        string filePath = System.IO.Path.Combine(Path, "empty.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsInvalidOperation_BadFormat()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_format.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsInvalidOperation_NonNumeric()
    {
        string filePath = System.IO.Path.Combine(Path, "bad_numeric.txt");
        Assert.Throws<InvalidOperationException>(() => new Matrix(1, 1).LoadFromFile(filePath));
    }

    [Fact]
    public void LoadFromFile_ThrowsFileNotFound_NotFound()
    {
        string filePath = System.IO.Path.Combine(Path, "file_not_found.txt");
        Assert.Throws<FileNotFoundException>(() => new Matrix(1, 1).LoadFromFile(filePath));
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
}