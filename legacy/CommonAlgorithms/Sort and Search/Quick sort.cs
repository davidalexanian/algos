// Contains the implementation of quick sort

using System.Collections.Generic;

namespace CommonAlgorithms.SortSearch
{   
    public static partial class SortSearch<T>
    {
        /// <summary>
        /// Sorts the collection using quick sort (expected complexity is O(N x Log(N)), wortscase is O(N^2)).
        /// </summary>        
        public static void QuickSort(IList<T> collection)
        {
            QuickSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using quick sort and provided comparison delegate (expected complexity is O(N x Log(N)), wortscase is O(N^2)).
        /// </summary>
        public static void QuickSort(IList<T> collection, System.Comparison<T> comparisonDelegate)
        {
            QuickSort(collection, Comparer<T>.Create(comparisonDelegate));
        }
        /// <summary>
        /// Sorts the collection using quick sort and provided comparer (expected complexity is O(N x Log(N)), wortscase is O(N^2))
        /// </summary>
        public static void QuickSort(IList<T> collection, Comparer<T> comparer)
        {
            QuickSortInner(collection, 0, collection.Count - 1, comparer);
        }
        /// <summary>
        /// Sort the input using quick sort algorithm
        /// </summary>
        private static void QuickSortInner(IList<T> collection, int start, int end, Comparer<T> comparer)
        {
            if (start < end)
            {
                int pivotIndex = GetPivotIndex(collection, start, end, comparer);    //after this, choosen pivot is in its place
                QuickSortInner(collection, start, pivotIndex-1, comparer);      //sort left part
                QuickSortInner(collection, pivotIndex+1, end, comparer);    //sort right part
            }
        }
        /// <summary>
        /// Returns the index of choosen pivot (the last element) in the final array (if the array was sorted, last element would have this index). 
        /// </summary>
        /// <remarks>
        /// After the call [start,index-1] elements are smaller or equal to pivot, [index+1,end] element are bigger.
        /// </remarks>
        private static int GetPivotIndex(IList<T> collection, int start, int end, Comparer<T> comparer)
        {
            int pivotIndex = start - 1;             // the index we search - 1
            T pivot = collection[end];              // choose last element as pivot            

            for (int i = start; i < end; i++)       // untill the last item exclusive
            {
                if (comparer.Compare(collection[i],pivot) <= 0)     // less or equal
                {
                    // [start,pivotIndex] elements are smaller than pivot, [pivotIndex+1,i] are bigger, others are not tested yet (loop invariant)
                    pivotIndex++;   // now pivotIndex might point to bigger value. Change with collection[i] to make sure loop invariant is preserved

                    // exchange pivotIndex & i 
                    T t = collection[pivotIndex];    
                    collection[pivotIndex] = collection[i];
                    collection[i] = t;
                }
            }

            pivotIndex = pivotIndex + 1;    //right place of pivot

            // put pivot's value in its right place (exchange pivotIndex & end)
            T temp = collection[pivotIndex];
            collection[pivotIndex] = pivot;
            collection[end] = temp;

            return pivotIndex;
        }
    }
}
