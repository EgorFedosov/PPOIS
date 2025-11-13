namespace L4;

internal abstract class Program
{
    private static void Main()
    {
        Person[] peopleArray =
        [
            new("Alice", 30),
            new("Bob", 25),
            new("Charlie", 35)
        ];

        Console.WriteLine("Исходный массив:");
        foreach (var p in peopleArray) Console.WriteLine(p);

        Sorting.CocktailSort(peopleArray);

        Console.WriteLine("\nОтсортированный массив (CocktailSort):");
        foreach (var p in peopleArray) Console.WriteLine(p);

        var peopleList = new List<Person>
        {
            new("Diana", 28),
            new("Eve", 22),
            new("Frank", 33)
        };

        Console.WriteLine("\nИсходный список:");
        peopleList.ForEach(Console.WriteLine);

        Sorting.StrandSort(peopleList);

        Console.WriteLine("\nОтсортированный список (StrandSort):");
        peopleList.ForEach(Console.WriteLine);
        
        var graph = new Graph<Person>();
        foreach (var p in peopleArray)
            graph.AddVertex(p);
        
        // добавляем ребра между вершинами
        var edges = new[] { (0, 1), (1, 2) };
        foreach (var (i, j) in edges)
            graph.AddEdge(peopleArray[i], peopleArray[j]);

        Console.WriteLine("\nГраф:");
        Console.WriteLine(graph);

        Console.WriteLine($"\nКоличество вершин: {graph.VertexCount}");
        Console.WriteLine($"Количество рёбер: {graph.EdgeCount}");
        
        foreach (var t in peopleArray)
            Console.WriteLine($"Степень вершины {t}: {graph.GetVertexDegree(t)}");
    }
}