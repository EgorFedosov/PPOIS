namespace L4;

public static class Sorting
{
    public static void CocktailSort<T>(T[] array)
    {
        var comparer = Comparer<T>.Default;
        bool sortOrNot;
        var right = array.Length - 1;
        var left = 1;

        do
        {
            sortOrNot = true;

            for (var i = left; i <= right; i++)
            {
                if (comparer.Compare(array[i - 1], array[i]) <= 0) continue;
                (array[i - 1], array[i]) = (array[i], array[i - 1]);
                sortOrNot = false;
            }

            right--;

            for (int i = right; i >= left; i--)
            {
                if (comparer.Compare(array[i], array[i - 1]) >= 0) continue;
                (array[i], array[i - 1]) = (array[i - 1], array[i]);
                sortOrNot = false;
            }

            left++;

        } while (sortOrNot == false);
    }
    
    public static void StrandSort<T>(List<T> list)
    {
        var comparer = Comparer<T>.Default;
        var result = new List<T>();

        while (list.Count > 0)
        {
            var strand = new List<T> { list[0] };
            list.RemoveAt(0);

            var i = 0;
            while (i < list.Count)
            {
                if (comparer.Compare(list[i], strand[^1]) >= 0)
                {
                    strand.Add(list[i]);
                    list.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            result = Merge(result, strand, comparer);
        }

        list.Clear();
        list.AddRange(result);
    }

    private static List<T> Merge<T>(List<T> a, List<T> b, IComparer<T> comparer)
    {
        var merged = new List<T>(a.Count + b.Count);
        int i = 0, j = 0;

        while (i < a.Count && j < b.Count)
        {
            if (comparer.Compare(a[i], b[j]) <= 0)
            {
                merged.Add(a[i]);
                i++;
            }
            else
            {
                merged.Add(b[j]);
                j++;
            }
        }

        while (i < a.Count) merged.Add(a[i++]);
        while (j < b.Count) merged.Add(b[j++]);

        return merged;
    }


}