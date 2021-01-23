using System;
using System.Collections.Generic;

namespace CommonAlgorithms.SortSearch
{
    // Contains common earching algorithms
    public static partial class SortSearch<T>
    {
        /// <summary>
        /// Returns the first occurance of specified value or -1 if the value is not found.
        /// </summary>        
        public static int LinearSearch(IEnumerable<T> collection, T value)
        {
            if (collection == null) throw new ArgumentNullException("Collection is null");
            int index = -1;
            foreach (var item in collection)
                if (item.Equals(value))
                {
                    index++;
                    break;
                }
            return index;
        }
        /// <summary>
        /// Search the collection for specified value by looping over it. Returns the index of first occurance or -1.
        /// </summary>
        public static int LinearSearch(IList<T> collection, T value, int startFrom = 0)
        {
            if (collection == null) throw new ArgumentNullException("Collection is null");
            if (startFrom >= collection.Count) throw new ArgumentException("Start from index is bigger than index of the collection's last item");

            for (int i = startFrom; i < collection.Count; i++)
                if (collection[i].Equals(value)) return i;
            return -1;
        }
        /// <summary>
        /// Search the collection for specified value by looping over it. Returns the index of first occurance or -1. Complexity is O(n)
        /// </summary>
        /// <param name="startFrom">Index to start searching the value from</param>
        /// <param name="isSorted">If true algorithm, assumes that array is sorted and returns as soon as value or an bigger element is found</param>
        public static int LinearSearch(IList<T> collection, T value, Comparer<T> comparer, int startFrom = 0, bool isSorted = false)
        {
            if (collection == null) throw new ArgumentNullException("Collection is null");
            if (startFrom >= collection.Count) throw new ArgumentException("Start from index is bigger than index of the collection's last item");
            if (isSorted && comparer == null) throw new ArgumentNullException("Comparer is not specified");

            for (int i = startFrom; i < collection.Count; i++)
            {
                int cResult = comparer.Compare(collection[i], value);
                if (cResult == 0) return i;
                if (cResult == 1) return -1;
            }
            return -1;
        }
        /// <summary>
        /// Searchs the sorted collection for specified value (complexity is O(logN)). Note that collection must be sorted.
        /// </summary>
        /// <returns>Index of the first occurance.-1 otherwise</returns>
        public static int BinarySearch(IList<T> collection, T value)
        {
            return BinarySearch(collection, value, Comparer<T>.Default);
        }
        /// <summary>
        /// Searchs the sorted collection for specified value (complexity is O(logN)). Note that collection must be sorted.
        /// </summary>
        /// <returns>Index of the first occurance.-1 otherwise</returns>
        public static int BinarySearch(IList<T> collection, T value, Comparer<T> comparer)
        {
            int min = 0, max = collection.Count - 1;

            while (min < max)
            {
                int mid = (min + max) / 2;
                int c = comparer.Compare(collection[mid], value);
                if (c == -1)
                    min = mid;      // middle element is smaller than the value, it is in right half
                else if (c == 1)
                    max = mid;      // middle element is bigger than the value, it is in left half
                else
                    return mid;     // found
            }
            if (min == max && collection[min].Equals(value)) return min;
            return -1;
        }
    }
}
