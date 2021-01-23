using System;
using System.Collections.Generic;

namespace CommonAlgorithms.SortSearch
{
    /// <summary>
    /// Contains common sorting and searching algorithms
    /// </summary>
    public static partial class SortSearch<T>
    {
        /// <summary>
        /// Sorts the collection using insertion sort and the default comparer of T (complexity is O(N^2)).
        /// </summary>        
        public static void InsertionSort(IList<T> collection)
        {
            InsertionSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using insertion sort and provided comparer (complexity is O(N^2)).
        /// </summary>
        public static void InsertionSort(IList<T> collection, IComparer<T> comparer)
        {
            InsertionSort(collection, comparer.Compare);
        }
        /// <summary>
        /// Sorts the collection using insertion sort and provided comparison delegate (complexity is O(N^2)).
        /// </summary>
        /// <remarks>
        /// The insertion sort algorithm works in the following way.
        ///     1. for each element i it keeps left array [0,i-1] sorted. Index i seperates sorted and unsorted parts of the array.
        ///     2. starting from i=1,it compares i with [0,i-1] to find bigger element
        ///     3. if found with index j, moves [j,i-1] elements to the right by one and inserts i-th element at index j
        /// </remarks>
        public static void InsertionSort(IList<T> collection, Comparison<T> comparisonDelegate)
        {
            if (collection == null) throw new ArgumentException("Collection is null");
            if (collection.Count == 0) return;
            for (int i = 1; i < collection.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (comparisonDelegate(collection[j], collection[i]) > 0)
                    {
                        T elem = collection[i];

                        for (int index = i-1; index >= j; index--)         //move[j, i - 1] elements to the right by one
                            collection[index + 1] = collection[index];

                        collection[j] = elem;                           // insert elem at index j
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Sorts the collection using selection sort and the default comparer of T (complexity is O(N^2)).
        /// </summary>
        public static void SelectionSort(IList<T> collection)
        {
            SelectionSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using selection sort and provided comparer (complexity is O(N^2)).
        /// </summary>
        public static void SelectionSort(IList<T> collection, IComparer<T> comparer)
        {
            SelectionSort(collection, comparer.Compare);
        }
        /// <summary>
        /// Sorts the collection using selection sort and provided comparison delegate (complexity is O(N^2)).
        /// </summary>
        /// <remarks>
        /// The selection sort algorithm works in the following way.
        ///     1. for each element i it keeps left array sorted [0,i-1]. Index i seperates sorted and unsorted parts of the array.
        ///     2. starting from i=0, algorithm finds i-th smallest element at index j and places it in i-th position (swaps i,j values)
        /// </remarks>
        public static void SelectionSort(IList<T> collection, Comparison<T> comparisonDelegate)
        {
            if (collection == null) throw new ArgumentException("Collection is null");
            if (collection.Count == 0) return;

            for (int i = 0; i < collection.Count; i++)
            {
                T smallestItem = collection[i];
                int smallestIndex = i;

                // find smallest element with index j >= i
                for (int j = i + 1; j < collection.Count; j++)      
                    if (comparisonDelegate(smallestItem, collection[j]) > 0)
                    { 
                        smallestItem = collection[j];
                        smallestIndex = j;
                    }

                collection[smallestIndex] = collection[i];
                collection[i] = smallestItem;       // swap place i-th smallest element in i-th position
            }
        }

        /// <summary>
        /// Sorts the collection using bubble sort and the default comparer of T (complexity is O(N^2)).
        /// </summary>
        public static void BubbleSort(IList<T> collection)
        {
            BubbleSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using bubble sort and provided comparer (complexity is O(N^2)).
        /// </summary>
        public static void BubbleSort(IList<T> collection, IComparer<T> comparer)
        {
            BubbleSort(collection, comparer.Compare);
        }
        /// <summary>
        /// Sorts the collection using bubble sort and provided comparison delegate (complexity is O(N^2)).
        /// </summary>
        /// <remarks>
        /// The bubble sort algorithm works in the following way.
        ///     1. Repeadetly checks adjacent elements looking for unordered pairs
        ///     2. If not found, sorted
        ///     3. If found, swaps the values and checks again.
        /// </remarks>
        public static void BubbleSort(IList<T> collection, Comparison<T> comparisonDelegate)
        {
            if (collection == null) throw new ArgumentException("Collection is null");
            if (collection.Count == 0) return;

            bool sorted = false;
            while (!sorted)
            {
                sorted = true;  //assume sorted. If unordered pair is found this will be reseted

                for (int j = 0; j < collection.Count-1; j++)    //a value is bubbled up to the right end of collection when for loop ends
                { 
                    if (comparisonDelegate(collection[j], collection[j+1]) > 0) 
                    {
                        // out of order pair found, swap values and mark as not sorted to check again
                        T temp = collection[j+1];
                        collection[j+1] = collection[j];
                        collection[j] = temp;
                        sorted = false;     
                    }
                }
            }
        }
        /// <summary>
        /// Implements counting sort (linear). Uses an extra storage. Good for sorting integers that lie in a relatively small range (e.g. sorting 1M numbers between 0...1000)
        /// </summary>
        public static void CountingSort(uint[] array)
        {
            if (array == null || array.Length == 0) return;

            // find the largest element (N steps)
            uint maxValue = array[0];
            foreach (uint i in array)
                if (i > maxValue) maxValue = i;

            // count appearance of each element (N steps)
            uint[] counts = new uint[maxValue + 1];
            foreach (uint i in array)
                counts[i]++;

            // copy values back to array (linear)
            uint index = 0;
            for (uint i = 0; i < counts.Length; i++)
            {
                // count[i]=0 if i is not in array, hence the control enters to inner loop max n times
                for (uint j = 1; j <= counts[i]; j++)
                {
                    array[index] = i;
                    index++;
                }
            }
        }

    }    
}
