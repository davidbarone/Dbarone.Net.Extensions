namespace Dbarone.Net.Extensions;

/// <summary>
/// Extension methods for collections and sequences.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns a statistically random subset of data.
    /// </summary>
    /// <typeparam name="T">The type of the data element.</typeparam>
    /// <param name="data">The data to provide a sample from.</param>
    /// <param name="sampleSize">The sample size.</param>
    /// <returns>A random sample of the data.</returns>
    public static IEnumerable<T> Sample<T>(this IEnumerable<T> data, int sampleSize)
    {
        List<int> sampleIndexes = new List<int>();
        int dataSize = data.Count();

        // Sample size cannot be greater than population / data size
        if (sampleSize > dataSize)
        {
            sampleSize = dataSize;
        }

        for (int i = 0; i < sampleSize; i++)
        {
            while (true)
            {
                var rand = new Random().Next(dataSize);
                if (!sampleIndexes.Contains(rand))
                {
                    sampleIndexes.Add(rand);
                    break;
                }
            }
        }

        var sample = sampleIndexes.Select(i => data.ElementAt(i));
        return sample;
    }

    /// <summary>
    /// Creates a cartesian product of n sequences.
    /// The resulting sequence contains all combinations.
    /// Each element has n items in the sequence.
    /// 
    /// Taken from: https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
    /// </summary>
    /// <typeparam name="T">The type of the element</typeparam>
    /// <param name="sequences">The data.</param>
    /// <returns>The cartesian product of elements.</returns>
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct =
          new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
          emptyProduct,
          (accumulator, sequence) =>
            from accseq in accumulator
            from item in sequence
            select accseq.Concat(new[] { item }));
    }

    /// <summary>
    /// Splices an array.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="source">The source array to splice.</param>
    /// <param name="start">The start element to splice elements from.</param>
    /// <param name="number">The number of elements to remove.</param>
    /// <returns>An array with the specified elements spliced from it.</returns>
    public static T[] Splice<T>(this T[] source, long start, long? number = null)
    {
        var len = source.Length;
        var c = len - start;
        if (number.HasValue)
            c = number.Value;
        T[] result = new T[c];

        while (c > 0)
        {
            result[c - 1] = source[start + c - 1];
            c--;
        }
        return result;
    }

    /// <summary>
    /// Creates a collection out of a set of objects. Individual items can themselves be collections or not.
    /// </summary>
    /// <param name="obj1">The current object.</param>
    /// <param name="obj2">A set of additional objects.</param>
    /// <returns>Returns a flattened (1 dimensional) collection of all the objects.</returns>
    public static IEnumerable<object> Union(this object obj1, params object[] obj2)
    {
        List<object> results = new List<object>();
        var enumerableObj1 = obj1 as System.Collections.IEnumerable;

        if (enumerableObj1 != null && obj1.GetType() != typeof(string))
        {
            foreach (var item in enumerableObj1)
                results.Add(item);
        }
        else if (obj1 != null)
            results.Add(obj1);
        else
            throw new Exception("Error: obj1 is invalid.");

        foreach (var objItem in obj2)
        {
            var enumerableItem = objItem as System.Collections.IEnumerable;
            if (enumerableItem != null && objItem.GetType() != typeof(string))
            {
                foreach (var item in enumerableItem)
                    results.Add(item);
            }
            else if (objItem != null)
                results.Add(objItem);
            else
                throw new Exception("Error: obj2 is invalid.");
        }
        return results;
    }
}
