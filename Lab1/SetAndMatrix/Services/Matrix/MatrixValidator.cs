namespace SetAndMatrix.Services.Matrix;

using Interfaces.Matrix;

public class MatrixValidator : IMatrixValidator
{
    public static void ValidateFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Путь не может быть null или пустым.", nameof(path));

        if (!File.Exists(path))
            throw new FileNotFoundException("Файл не найден.", path);

        var lines = File.ReadAllLines(path);
        if (lines.Length == 0)
            throw new InvalidOperationException("Файл пустой.");

        int cols = -1;

        foreach (var line in lines)
        {
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (cols == -1)
                cols = values.Length;
            else if (values.Length != cols)
                throw new InvalidOperationException("Строки имеют разное количество элементов.");

            foreach (var val in values)
            {
                if (!double.TryParse(val, out _))
                    throw new InvalidOperationException($"Некорректное число: '{val}'");
            }
        }
    }
}