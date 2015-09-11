using ICollectionExtensions;
using System.Collections.Generic;
using System.Linq;

public class NonContiguousData<T>
{
    static int GetNextNonContiguous(ICollection<int> indices, int currentIndex)
    {
        var sortedIndices = new List<int>(indices);
        sortedIndices.Sort();

        var higherKeys = (from i in sortedIndices
                          where i >= currentIndex + 1
                          select i).ToList<int>();
        // if there are no higher values, reset so that the values loop
        if (higherKeys.Empty())
        {
            higherKeys = sortedIndices;
        }

        return higherKeys[0];
    }

    int index;
    Dictionary<int, T> data = new Dictionary<int, T>();

    public T this[int index]
    {
        get
        {
            return data[index];
        }

        set
        {
            data[index] = value;
        }
    }

    public bool Empty()
    {
        return data.Count <= 0;
    }

    public bool Remove(int index)
    {
        return data.Remove(index);
    }

    public T Next()
    {
        return this[IncreaseIndex()];
    }

    int NextIndex()
    {
        return GetNextNonContiguous(data.Keys, index);
    }

    int IncreaseIndex()
    {
        index = NextIndex();
        return index;
    }
}
