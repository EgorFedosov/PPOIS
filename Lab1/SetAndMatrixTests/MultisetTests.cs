namespace SetAndMatrixTests;

using Xunit;
using SetAndMatrix.Models.Multiset;

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
    public void Multiset_Add_AddsElementCorrectly()
    {
        Multiset multiset = new Multiset();
        MultisetElement element1 = new MultisetElement("a");
        MultisetElement element2 = new MultisetElement("b");

        multiset.Add(element1);
        multiset.Add(element2);

        Assert.Equal("{a,b}", multiset.ToString());
    }

    [Fact]
    public void Multiset_ToString_HandlesEmptyMultiset()
    {
        Multiset multiset = new Multiset();
        Assert.Equal("{}", multiset.ToString());
    }

    [Fact]
    public void Multiset_ToString_HandlesNestedMultiset()
    {
        Multiset innerMultiset = new Multiset();
        innerMultiset.Add(new MultisetElement("x"));
        innerMultiset.Add(new MultisetElement("y"));

        Multiset outerMultiset = new Multiset();
        outerMultiset.Add(new MultisetElement("a"));
        outerMultiset.Add(new MultisetElement(innerMultiset));

        Assert.Equal("{a,{x,y}}", outerMultiset.ToString());
    }

    [Fact]
    public void Multiset_ToString_HandlesMixedElements()
    {
        Multiset innerMultiset = new Multiset();
        innerMultiset.Add(new MultisetElement("1"));

        Multiset multiset = new Multiset();
        multiset.Add(new MultisetElement("alpha"));
        multiset.Add(new MultisetElement(innerMultiset));
        multiset.Add(new MultisetElement("beta"));

        Assert.Equal("{alpha,{1},beta}", multiset.ToString());
    }
} 