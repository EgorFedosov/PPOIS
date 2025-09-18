namespace SetAndMatrixTests;

using SetAndMatrix.Models;

public class MultisetTests
{
    [Fact]
    public void MultisetElement_StringConstructor_SetsElementCorrectly()
    {
        MultisetElement element = new MultisetElement("test");
        Assert.Equal("test", element.Element);
        Assert.Null(element.Nested);
    }

    [Fact]
    public void MultisetElement_NestedMultisetConstructor_SetsNestedCorrectly()
    {
        Multiset nestedMultiset = new Multiset();
        MultisetElement element = new MultisetElement(nestedMultiset);
        Assert.Same(nestedMultiset, element.Nested);
        Assert.Null(element.Element);
    }


    [Fact]
    public void Multiset_ToString_HandlesEmptyMultiset()
    {
        Multiset multiset = new Multiset();
        Assert.Equal("{}", multiset.ToString());
    }

    [Fact]
    public void Multiset_Parse_HandlesNestedMultiset()
    {
        Multiset multiset = new Multiset();
        Multiset parsed = multiset.Parse("{a,{x,y}}");
        Assert.Equal("{a,{x,y}}", parsed.ToString());
    }

    [Fact]
    public void Multiset_Parse_HandlesMixedElements()
    {
        Multiset multiset = new Multiset();
        Multiset parsed = multiset.Parse("{alpha,{1},beta}");
        Assert.Equal("{alpha,{1},beta}", parsed.ToString());
    }

    [Fact]
    public void Multiset_Parse_ThrowsOnInvalidInput()
    {
        Multiset multiset = new Multiset();
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("{a,,b}"));
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("{a,b"));
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("a,b}"));
    }
}