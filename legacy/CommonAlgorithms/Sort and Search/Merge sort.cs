using System.Collections.Generic;

namespace CommonAlgorithms.SortSearch
{
    // Contains the implementation of merge sort
    public static partial class SortSearch<T>
    {
        /// <summary>
        /// Sorts the collection using merge sort (complexity is O(N x Log(N)))
        /// </summary>        
        public static void MergeSort(IList<T> collection)
        {
            MergeSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using merge sort and provided comparison delegate (complexity is O(N x Log(N)))
        /// </summary>
        public static void MergeSort(IList<T> collection, System.Comparison<T> comparisonDelegate)
        {
            MergeSort(collection, Comparer<T>.Create(comparisonDelegate));
        }
        /// <summary>
        /// Sorts the collection using merge sort and provided comparer (complexity is O(N x Log(N)))
        /// </summary>
        public static void MergeSort(IList<T> collection, Comparer<T> comparer)
        {
            MergeSortInner(collection,0,collection.Count-1,comparer);
        }
        /// <summary>
        /// Sort the input using quick sort algorithm
        /// </summary>
        private static void MergeSortInner(IList<T> collection, int start, int end, Comparer<T> comparer)
        {
            if (start >= end) return;   // 1 element collection is sorted
            int midPoint = (start + end) / 2;
            MergeSortInner(collection, start, midPoint, comparer);
            MergeSortInner(collection, midPoint+1, end, comparer);
            MergeTwoSortedArrays(collection, start, end, midPoint, comparer);
        }
        /// <summary>
        /// Merges to sorted arrays with indexes [start,midpoint] and [midpoint+1,end] in given collection. Uses extra array.
        /// </summary>
        private static void MergeTwoSortedArrays(IList<T> collection, int start, int end, int midPoint, Comparer<T> comparer)
        {
            int leftIndex = start; 
            int rightIndex = midPoint + 1;
            int arrayIndex = leftIndex;

            // loop the halves & copy the smaller item from whichever half holds it into the new array
            T[] array = new T[collection.Count];
            while (leftIndex <= midPoint && rightIndex <= end)
            {
                if (comparer.Compare(collection[leftIndex], collection[rightIndex]) <= 0)
                {
                    array[arrayIndex] = collection[leftIndex];
                    leftIndex++;
                }
                else
                {
                    array[arrayIndex] = collection[rightIndex];
                    rightIndex++;
                }
                arrayIndex++;
            }

            // copy remaining items
            for (int index = leftIndex; index <= midPoint; index++)
                array[arrayIndex++]= collection[index];

            for (int index = rightIndex; index <= end; index++)
                array[arrayIndex++] = collection[index];

            // copy back to collection from array
            for (int index = start; index <= end; index++)
                collection[index] = array[index];
        }
    }
}
