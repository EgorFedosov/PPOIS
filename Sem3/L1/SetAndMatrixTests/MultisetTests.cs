namespace SetAndMatrixTests;

using SetAndMatrix.Models;

public class MultisetTests
{
    [Fact]
    public void StringElement_CreatedCorrectly()
    {
        MultisetElement element = new MultisetElement("test");
        Assert.Equal("test", element.Element);
        Assert.Null(element.Nested);
    }

    [Fact]
    public void NestedElement_CreatedCorrectly()
    {
        Multiset nestedMultiset = new Multiset();
        MultisetElement element = new MultisetElement(nestedMultiset);
        Assert.Same(nestedMultiset, element.Nested);
        Assert.Null(element.Element);
    }

    [Fact]
    public void EmptyMultiset_ToString_ReturnsEmptyBraces()
    {
        Multiset multiset = new Multiset();
        Assert.Equal("{}", multiset.ToString());
    }

    [Fact]
    public void Parse_NestedMultiset_WorksCorrectly()
    {
        Multiset multiset = new Multiset();
        Multiset parsed = multiset.Parse("{a,{x,y}}");
        Assert.Equal("{a,{x,y}}", parsed.ToString());
    }

    [Fact]
    public void Parse_SimpleElements_WorksCorrectly()
    {
        Multiset multiset = new Multiset("{a,b}");
        Assert.Equal("{a,b}", multiset.ToString());
    }

    [Fact]
    public void Parse_MixedElements_WorksCorrectly()
    {
        Multiset multiset = new Multiset();
        Multiset parsed = multiset.Parse("{alpha,{1},beta}");
        Assert.Equal("{alpha,{1},beta}", parsed.ToString());
    }

    [Fact]
    public void Parse_InvalidInput_ThrowsExceptions()
    {
        Multiset multiset = new Multiset();
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("{a,,b}"));
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("{a,b"));
        Assert.Throws<InvalidOperationException>(() => multiset.Parse("a,b}"));
    }
}