using L4;

namespace L4Tests;

public class Tests
{
    private readonly Person _p1 = new("Alice", 30);
    private readonly Person _p2 = new("Bob", 25);
    private readonly Person _p3 = new("Charlie", 35);
    private readonly Person _p4 = new("Diana", 28);

    [Fact]
    public void Person_ToString_IsCorrect()
    {
        var person = new Person("Test", 99);
        Assert.Equal("Test (99)", person.ToString());
    }

    [Fact]
    public void Person_Equals_Works()
    {
        var personA = new Person("Test", 99);
        var personB = new Person("Test", 99);
        var personC = new Person("Other", 99);
        var personD = new Person("Test", 100);

        Assert.True(personA.Equals(personB));
        Assert.False(personA.Equals(personC));
        Assert.False(personA.Equals(personD));
        Assert.False(personA.Equals(null));
        Assert.False(personA.Equals(new object()));
    }

    [Fact]
    public void Person_CompareTo_UsesAge()
    {
        Assert.True(_p2.CompareTo(_p1) < 0);
        Assert.True(_p1.CompareTo(_p2) > 0);
        Assert.True(_p1.CompareTo(new Person("SameAge", 30)) == 0);
        Assert.True(_p1.CompareTo(null) > 0);
    }

    [Fact]
    public void Person_GetHashCode_IsSameForSamePerson()
    {
        var personA = new Person("Test", 99);
        var personB = new Person("Test", 99);
        var personC = new Person("Other", 100);

        Assert.Equal(personA.GetHashCode(), personB.GetHashCode());
        Assert.NotEqual(personA.GetHashCode(), personC.GetHashCode());
    }

    private Person[] GetUnsortedPeople() =>
    [
        new("Charlie", 35),
        new("Alice", 30),
        new("Eve", 22),
        new("Bob", 25),
        new("Diana", 28)
    ];
    
    private List<Person> GetUnsortedPeople_List() => GetUnsortedPeople().ToList();

    private Person[] GetSortedPeople() =>
    [
        new("Eve", 22),
        new("Bob", 25),
        new("Diana", 28),
        new("Alice", 30),
        new("Charlie", 35)
    ];

    [Fact]
    public void CocktailSort_SortsArray()
    {
        var array = GetUnsortedPeople();
        var expected = GetSortedPeople();
        
        Sorting.CocktailSort(array);
        
        Assert.Equal(expected, array);
    }
    
    [Fact]
    public void CocktailSort_EmptyArray()
    {
        var array = Array.Empty<Person>();
        Sorting.CocktailSort(array);
        Assert.Empty(array);
    }

    [Fact]
    public void CocktailSort_OnePersonArray()
    {
        var array = new[] { _p1 };
        Sorting.CocktailSort(array);
        Assert.Single(array);
        Assert.Equal(_p1, array[0]);
    }
    
    [Fact]
    public void CocktailSort_SortedArray()
    {
        var array = GetSortedPeople();
        var expected = GetSortedPeople();
        
        Sorting.CocktailSort(array);
        
        Assert.Equal(expected, array);
    }

    [Fact]
    public void CocktailSort_ReversedArray()
    {
        var array = GetSortedPeople().Reverse().ToArray();
        var expected = GetSortedPeople();
        
        Sorting.CocktailSort(array);
        
        Assert.Equal(expected, array);
    }
    
    [Fact]
    public void StrandSort_SortsList()
    {
        var list = GetUnsortedPeople_List();
        var expected = GetSortedPeople().ToList();
        
        Sorting.StrandSort(list);
        
        Assert.Equal(expected, list);
    }

    [Fact]
    public void StrandSort_EmptyList()
    {
        var list = new List<Person>();
        Sorting.StrandSort(list);
        Assert.Empty(list);
    }

    [Fact]
    public void StrandSort_OnePersonList()
    {
        var list = new List<Person> { _p1 };
        Sorting.StrandSort(list);
        Assert.Single(list);
        Assert.Equal(_p1, list[0]);
    }

    [Fact]
    public void StrandSort_SortedList()
    {
        var list = GetSortedPeople().ToList();
        var expected = GetSortedPeople().ToList();
        
        Sorting.StrandSort(list);
        
        Assert.Equal(expected, list);
    }
    
    [Fact]
    public void StrandSort_ReversedList()
    {
        var list = GetSortedPeople().Reverse().ToList();
        var expected = GetSortedPeople().ToList();
        
        Sorting.StrandSort(list);
        
        Assert.Equal(expected, list);
    }

    [Fact]
    public void Graph_New_IsEmpty()
    {
        var graph = new Graph<Person>();
        
        Assert.Equal(0, graph.VertexCount);
        Assert.Equal(0, graph.EdgeCount);
        Assert.True(graph.Empty);
    }
    
    [Fact]
    public void Graph_Copy_IsNewCopy()
    {
        var original = new Graph<Person>();
        original.AddVertex(_p1);
        original.AddVertex(_p2);
        original.AddEdge(_p1, _p2);

        var copy = new Graph<Person>(original);
        
        Assert.True(original.Equals(copy));
        Assert.Equal(original.VertexCount, copy.VertexCount);
        Assert.Equal(original.EdgeCount, copy.EdgeCount);
        Assert.True(copy.ContainsEdge(_p1, _p2));

        copy.AddVertex(_p3);
        copy.AddEdge(_p2, _p3);
        
        Assert.Equal(2, original.VertexCount);
        Assert.Equal(1, original.EdgeCount);
        Assert.False(original.ContainsVertex(_p3));
        
        Assert.Equal(3, copy.VertexCount);
        Assert.Equal(2, copy.EdgeCount);
        Assert.True(copy.ContainsVertex(_p3));
        
        original.RemoveEdge(_p1, _p2);
        Assert.Equal(0, original.EdgeCount);
        Assert.Equal(2, copy.EdgeCount);
    }
    
    [Fact]
    public void Graph_AddVertex_AddsPerson()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.Equal(1, graph.VertexCount);
        Assert.False(graph.Empty);
        Assert.True(graph.ContainsVertex(_p1));
    }

    [Fact]
    public void Graph_AddSameVertex_Fails()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.Throws<InvalidOperationException>(() => graph.AddVertex(_p1));
    }

    [Fact]
    public void Graph_RemoveVertex_RemovesPersonAndLinks()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddVertex(_p3);
        graph.AddEdge(_p1, _p2);
        graph.AddEdge(_p2, _p3);
        
        Assert.Equal(3, graph.VertexCount);
        Assert.Equal(2, graph.EdgeCount);
        Assert.True(graph.ContainsEdge(_p1, _p2));

        graph.RemoveVertex(_p2);
        
        Assert.Equal(2, graph.VertexCount);
        Assert.Equal(0, graph.EdgeCount);
        Assert.False(graph.ContainsVertex(_p2));
        Assert.True(graph.ContainsVertex(_p1));
        Assert.True(graph.ContainsVertex(_p3));
        Assert.False(graph.ContainsEdge(_p1, _p2));
        Assert.False(graph.ContainsEdge(_p2, _p3));
    }

    [Fact]
    public void Graph_RemoveMissingVertex_Fails()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.Throws<InvalidOperationException>(() => graph.RemoveVertex(_p2));
    }

    [Fact]
    public void Graph_AddEdge_LinksBothWays()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddEdge(_p1, _p2);
        
        Assert.Equal(1, graph.EdgeCount);
        Assert.True(graph.ContainsEdge(_p1, _p2));
        Assert.True(graph.ContainsEdge(_p2, _p1));
    }
    
    [Fact]
    public void Graph_AddEdge_MissingVertex_Fails()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.Throws<InvalidOperationException>(() => graph.AddEdge(_p1, _p2));
        Assert.Throws<InvalidOperationException>(() => graph.AddEdge(_p2, _p1));
    }

    [Fact]
    public void Graph_RemoveEdge_RemovesLink()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddEdge(_p1, _p2);
        
        Assert.Equal(1, graph.EdgeCount);
        
        graph.RemoveEdge(_p1, _p2);
        
        Assert.Equal(0, graph.EdgeCount);
        Assert.False(graph.ContainsEdge(_p1, _p2));
        Assert.False(graph.ContainsEdge(_p2, _p1));
    }
    
    [Fact]
    public void Graph_RemoveEdge_MissingVertex_Fails()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.Throws<InvalidOperationException>(() => graph.RemoveEdge(_p1, _p2));
    }

    [Fact]
    public void Graph_ContainsEdge_MissingVertex_IsFalse()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        
        Assert.False(graph.ContainsEdge(_p1, _p2));
        Assert.False(graph.ContainsEdge(_p2, _p1));
        Assert.False(graph.ContainsEdge(_p2, _p3));
    }
    
    [Fact]
    public void Graph_GetVertexDegree_CountsLinks()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddVertex(_p3);
        graph.AddVertex(_p4);
        
        graph.AddEdge(_p1, _p2);
        graph.AddEdge(_p1, _p3);
        
        Assert.Equal(2, graph.GetVertexDegree(_p1));
        Assert.Equal(1, graph.GetVertexDegree(_p2));
        Assert.Equal(1, graph.GetVertexDegree(_p3));
        Assert.Equal(0, graph.GetVertexDegree(_p4));
    }

    [Fact]
    public void Graph_GetVertexDegree_MissingVertex_Fails()
    {
        var graph = new Graph<Person>();
        Assert.Throws<InvalidOperationException>(() => graph.GetVertexDegree(_p1));
    }
    
    [Fact]
    public void Graph_Clear_MakesEmpty()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddEdge(_p1, _p2);
        
        graph.Clear();
        
        Assert.Equal(0, graph.VertexCount);
        Assert.Equal(0, graph.EdgeCount);
        Assert.True(graph.Empty);
        Assert.False(graph.ContainsVertex(_p1));
        Assert.False(graph.ContainsEdge(_p1, _p2));
    }

    [Fact]
    public void Graph_GetEnumerator_LoopsPeople()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddVertex(_p3);
        
        var vertices = new List<Person>();
        foreach (var v in graph)
        {
            vertices.Add(v);
        }
        
        Assert.Equal(3, vertices.Count);
        Assert.Contains(_p1, vertices);
        Assert.Contains(_p2, vertices);
        Assert.Contains(_p3, vertices);
        Assert.Equal(new[] { _p1, _p2, _p3 }, vertices);
    }

    [Fact]
    public void Graph_Equals_Works()
    {
        var graphA = new Graph<Person>();
        graphA.AddVertex(_p1);
        graphA.AddVertex(_p2);
        graphA.AddEdge(_p1, _p2);
    
        var graphB = new Graph<Person>();
        graphB.AddVertex(_p1);
        graphB.AddVertex(_p2);
        graphB.AddEdge(_p1, _p2);
    
        var graphCDiffPeople = new Graph<Person>();
        graphCDiffPeople.AddVertex(_p1);
        graphCDiffPeople.AddVertex(_p3);
        graphCDiffPeople.AddEdge(_p1, _p3);
    
        var graphDDiffLinks = new Graph<Person>();
        graphDDiffLinks.AddVertex(_p1);
        graphDDiffLinks.AddVertex(_p2);
    
        var graphEDiffOrder = new Graph<Person>();
        graphEDiffOrder.AddVertex(_p2);
        graphEDiffOrder.AddVertex(_p1);
        graphEDiffOrder.AddEdge(_p1, _p2);
    
        Assert.True(graphA.Equals(graphB));
        Assert.True(graphA == graphB);
        Assert.False(graphA != graphB);
    
        Assert.False(graphA.Equals(null));
        Assert.False(graphA.Equals(graphCDiffPeople));
        Assert.False(graphA.Equals(graphDDiffLinks));
        Assert.False(graphA.Equals(graphEDiffOrder));
        Assert.False(graphA == graphDDiffLinks);
        Assert.True(graphA != graphDDiffLinks);
    }
    
    [Fact]
    public void Graph_GetHashCode_IsSameForSamePeople()
    {
        var graphA = new Graph<Person>();
        graphA.AddVertex(_p1);
        graphA.AddVertex(_p2);
        graphA.AddEdge(_p1, _p2);
        
        var graphB = new Graph<Person>();
        graphB.AddVertex(_p1);
        graphB.AddVertex(_p2);
        graphB.AddEdge(_p1, _p2);

        var graphCDiffLinks = new Graph<Person>();
        graphCDiffLinks.AddVertex(_p1);
        graphCDiffLinks.AddVertex(_p2);

        var graphDDiffPeople = new Graph<Person>();
        graphDDiffPeople.AddVertex(_p1);
        graphDDiffPeople.AddVertex(_p3);

        Assert.Equal(graphA.GetHashCode(), graphB.GetHashCode());
        Assert.Equal(graphA.GetHashCode(), graphCDiffLinks.GetHashCode()); 
        Assert.NotEqual(graphA.GetHashCode(), graphDDiffPeople.GetHashCode());
    }
    
    [Fact]
    public void Graph_ToString_IsCorrect()
    {
        var graph = new Graph<Person>();
        graph.AddVertex(_p1);
        graph.AddVertex(_p2);
        graph.AddVertex(_p3);
        graph.AddEdge(_p1, _p2);
        graph.AddEdge(_p1, _p3);

        var expected = string.Join(Environment.NewLine,
            "Alice (30): Bob (25), Charlie (35)",
            "Bob (25): Alice (30)",
            "Charlie (35): Alice (30)"
        );

        Assert.Equal(expected, graph.ToString());
    }
}