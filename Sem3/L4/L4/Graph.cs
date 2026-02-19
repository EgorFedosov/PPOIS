using System.Collections;

namespace L4;

public class Graph<T> : IEnumerable<T>, IEquatable<Graph<T>>
{
    private readonly List<T> _vertices;
    private bool[,] _adjacency;
    public int VertexCount => _vertices.Count;
    public bool Empty => _vertices.Count == 0;
    public bool ContainsVertex(T v) => _vertices.Contains(v);

    public Graph()
    {
        _vertices = [];
        _adjacency = new bool[0, 0];
    }

    public Graph(Graph<T> other)
    {
        _vertices = new List<T>(other._vertices);
        _adjacency = (bool[,])other._adjacency.Clone();
    }

    public int EdgeCount
    {
        get
        {
            var count = 0;
            for (var i = 0; i < VertexCount; i++)
            for (var j = i + 1; j < VertexCount; j++)
                if (_adjacency[i, j]) count++;
            return count;
        }
    }
    public bool ContainsEdge(T v1, T v2)
    {
        var i = _vertices.IndexOf(v1);
        var j = _vertices.IndexOf(v2);
        if (i == -1 || j == -1) return false;
        return _adjacency[i, j];
    }


    public void Clear()
    {
        _vertices.Clear();
        _adjacency = new bool[0, 0];
    }

    public void AddVertex(T value)
    {
        if (_vertices.Contains(value))
            throw new InvalidOperationException("Vertex already exists.");

        _vertices.Add(value);
        var n = _vertices.Count;
        var newAdj = new bool[n, n];

        for (var i = 0; i < n - 1; i++)
        for (var j = 0; j < n - 1; j++)
            newAdj[i, j] = _adjacency[i, j];

        _adjacency = newAdj;
    }

    public void RemoveVertex(T value)
    {
        var index = _vertices.IndexOf(value);
        if (index == -1)
            throw new InvalidOperationException("Vertex not found.");

        _vertices.RemoveAt(index);
        var n = _vertices.Count;
        var newAdj = new bool[n, n];

        for (int i = 0, ni = 0; i < n + 1; i++)
        {
            if (i == index) continue;
            for (int j = 0, nj = 0; j < n + 1; j++)
            {
                if (j == index) continue;
                newAdj[ni, nj] = _adjacency[i, j];
                nj++;
            }
            ni++;
        }

        _adjacency = newAdj;
    }

    public void AddEdge(T v1, T v2)
    {
        var i = _vertices.IndexOf(v1);
        var j = _vertices.IndexOf(v2);
        if (i == -1 || j == -1)
            throw new InvalidOperationException("Vertex not found.");
        _adjacency[i, j] = _adjacency[j, i] = true;
    }

    public void RemoveEdge(T v1, T v2)
    {
        var i = _vertices.IndexOf(v1);
        var j = _vertices.IndexOf(v2);
        if (i == -1 || j == -1)
            throw new InvalidOperationException("Vertex not found.");
        _adjacency[i, j] = _adjacency[j, i] = false;
    }



    public int GetVertexDegree(T v)
    {
        var index = _vertices.IndexOf(v);
        if (index == -1) throw new InvalidOperationException("Vertex not found.");
        var degree = 0;
        for (var i = 0; i < VertexCount; i++)
            if (_adjacency[index, i]) degree++;
        return degree;
    }

    public IEnumerator<T> GetEnumerator() => _vertices.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(Graph<T>? other)
    {
        if (other is null) return false;
        if (VertexCount != other.VertexCount) return false;

        for (var i = 0; i < VertexCount; i++)
            if (!EqualityComparer<T>.Default.Equals(_vertices[i], other._vertices[i]))
                return false;

        for (var i = 0; i < VertexCount; i++)
        for (var j = 0; j < VertexCount; j++)
            if (_adjacency[i, j] != other._adjacency[i, j])
                return false;

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as Graph<T>);

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var v in _vertices)
        {
            hash = hash * 31 + (v?.GetHashCode() ?? 0);
        }
        return hash;
    }

    public static bool operator ==(Graph<T> a, Graph<T> b) => EqualityComparer<Graph<T>>.Default.Equals(a, b);
    public static bool operator !=(Graph<T> a, Graph<T> b) => !(a == b);

    public override string ToString()
    {
        var lines = new List<string>();
        for (var i = 0; i < VertexCount; i++)
        {
            var edges = new List<string?>();
            for (var j = 0; j < VertexCount; j++)
                if (_adjacency[i, j])
                    edges.Add(_vertices[j]?.ToString());
            lines.Add($"{_vertices[i]}: {string.Join(", ", edges)}");
        }
        return string.Join(Environment.NewLine, lines);
    }
}