// File contains implementation of heap sort algorithm based on data structure called max heap which has ability to sort array in place.
// Heap is almost complete binary tree with elements stored in array. Max heap is a structure where the value of 
// every node is at least as large as the values of its children. 

using System.Collections.Generic;

namespace CommonAlgorithms.SortSearch
{
    public partial class SortSearch<T>
    {
        /// <summary>
        /// Sorts the collection using heap sort (complexity is O(N x Log(N)))
        /// </summary>
        public static void HeapSort(IList<T> collection)
        {
            HeapSort(collection, Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the collection using heap sort (complexity is O(N x Log(N)))
        /// </summary>
        public static void HeapSort(IList<T> collection, System.Comparison<T> comp)
        {
            HeapSort(collection, Comparer<T>.Create(comp));
        }
        /// <summary>
        /// Sorts the collection using heap sort (complexity is O(N x Log(N)))
        /// </summary>
        public static void HeapSort(IList<T> collection, Comparer<T> comparer)
        {
            if (collection == null || collection.Count == 0) return;
            MakeHeap(collection, comparer);     // make unordered input a valid heap

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                // swap root with collection i (biggets element goes to the end)
                T temp = collection[0]; 
                collection[0] = collection[i];              // this breaks heap property of the root node
                collection[i] = temp;

                HeapifyNode(collection, 0, i, comparer);    // restore the heap property of the root
            }
        }
        /// <summary>
        /// Turns collection into a valid max heap that is, the value of every node is at least as large as the values of its children (complexity is O(n x log(n)).
        /// </summary>
        private static void MakeHeap(IList<T> collection, Comparer<T> comparer)
        {
            for (int i = 1; i < collection.Count; i++)  // add each item to heap one by one
            {
                int index = i;
                // move current item up to the root while it is bigger than its parent. [0,index-1] subarray is a valid heap
                while (index != 0)  //complexity of while loop log(n) = height of heap
                {
                    int parent = GetParentIndex(index);
                    if (comparer.Compare(collection[index], collection[parent]) > 0)
                    {
                        // swap index with parent
                        T temp = collection[index];             
                        collection[index] = collection[parent];
                        collection[parent] = temp;

                        // check changed parent and swap value of parent with its parent if heap is broken, untill we reach the root (bigest element currently in heap)
                        index = parent;                         
                    }
                    else
                        break;
                }
            }
        }
        /// <summary>
        /// The function makes sure that the given collection is valid max heap, that is, the value of every node is at least as large as 
        /// the values of its children (complexity is LOG(n)).
        /// </summary>
        /// <param name="collection">Collection where heap is stored</param>
        /// <param name="nodeIndex">Index of the node to heapify</param>
        /// <param name="heapSize"></param>
        /// <remarks>The procedure assumes that left and right subtrees of given node are valid max heaps. 
        /// However, the value of the node might still be less from its children. The procedures corrects that by repetitively 
        /// swaping node's value down by the tree.
        /// </remarks>
        private static void HeapifyNode(IList<T> collection, int nodeIndex, int heapSize, Comparer<T> comparer)
        {
            if (collection == null) return;
            if (collection.Count == 0) return;

            // find largest child that is in heap
            int left = GetLeftIndex(nodeIndex);
            int right = GetRightIndex(nodeIndex);
            int largest = nodeIndex;
            
            // index 
            if (left < heapSize && comparer.Compare(collection[left], collection[nodeIndex]) == 1) largest = left;
            if (right < heapSize && comparer.Compare(collection[right], collection[largest]) == 1) largest = right;

            if (largest != nodeIndex)
            {
                // exchange values of index & largest nodes
                T temp = collection[largest];
                collection[largest] = collection[nodeIndex];
                collection[nodeIndex] = temp;

                // make sure that subtree rooted at largest index is max heap
                HeapifyNode(collection, largest, heapSize, comparer);
            }
        }
        /// <summary>
        /// Returns the index of left node of the given node (complexity is O(1))
        /// </summary>
        private static int GetLeftIndex(int nodeIndex)
        {
            return nodeIndex * 2 + 1;
        }
        /// <summary>
        /// Returns the index of right node of the given node (complexity is O(1))
        /// </summary>
        private static int GetRightIndex(int nodeIndex)
        {
            return nodeIndex * 2 + 2;
        }
        /// <summary>
        /// Returns the index of parent node of the given node (complexity is O(1))
        /// </summary>
        private static int GetParentIndex(int nodeIndex)
        {
            if (nodeIndex == 0) return 0;
            return (int)System.Math.Floor(((double)(nodeIndex - 1)) / 2);
        }
    }
}
