using System;
using System.Collections.Generic;
using CommonAlgorithms.Numerical;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents a hastable with chaining which can contain duplicate values. Buckets are implemented as doubly linked lists.
    /// </summary>
    /// <remarks>
    /// Hastable uses primes as bucket capacity to ensure better distribution of bucket indexes. Hashtable automatically increases the buckets capacity
    /// if actual load factor exceeds the maximum load factor. As duplicate values are allowed, the worst case scenario occurs when duplicate values 
    /// are repeatedly added to the collection. In this case the size of buckets array still increases leaving many empty buckets.
    /// </remarks>
    public class HashTable<T>
    {
        #region "Constructors,properties and fields"
        /// <summary>
        /// Initializes hashtable with initial capacity 17 and default equality comparer of T
        /// </summary>
        public HashTable() : this(17, EqualityComparer<T>.Default) { }
        /// <summary>
        /// Initializes hashtable with initial capacity 17 and provided equality comparer of T
        /// </summary>
        public HashTable(EqualityComparer<T> equalityComparer) :this(17,equalityComparer) { }
        /// <summary>
        /// Initializes hashtable with default equality comparer of T and capacity equal to smallest prime number bigger than provided value
        /// </summary>        
        public HashTable(int capacity) : this(capacity, EqualityComparer<T>.Default) {}
        /// <summary>
        /// Initializes hashtable with given equality comparer of T and capacity equal to smallest prime number bigger than provided value.
        /// </summary>
        public HashTable(int capacity, EqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null) throw new ArgumentNullException("Comparer is null");
            this.equalityComparer = equalityComparer;
            if (capacity < 0) throw new ArgumentException("Capacity can't be negative");

            // set buckets capacity to next smallest prime
            if (capacity < 17)
                capacity = 17;
            else
            {
                ulong cap = NumericAlgorithms.FindNextPrime((ulong)capacity);
                capacity = cap > int.MaxValue ? int.MaxValue : (int)cap;        //int.MaxValue is prime
            }
            bucketsCapacity = capacity;
            buckets = new LinkedList<T>[bucketsCapacity];
        }
        /// <summary>
        /// Initializes hashtable with default equality comparer and capacity equal to smallest prime number bigger than collection.count
        /// </summary>
        public HashTable(IEnumerable<T> collection) : this(collection, EqualityComparer<T>.Default) { }
        /// <summary>
        /// Initializes hashtable with given equality comparer and capacity equal to smallest prime number bigger than collection.count
        /// </summary>
        public HashTable(IEnumerable<T> collection, EqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null) throw new ArgumentNullException("Comparer is null");
            this.equalityComparer = equalityComparer;

            // set buckets capacity to next smallest prime
            if (collection == null) throw new ArgumentNullException("Collection is null");
            if (collection is IList<T>)
                bucketsCapacity = (collection as IList<T>).Count;
            else
            {
                int cap = 0;
                foreach (var item in collection)
                    cap++;
                bucketsCapacity = cap;
            }
            if (bucketsCapacity < 17)
                bucketsCapacity = 17;
            else
            {
                ulong cap = NumericAlgorithms.FindNextPrime((ulong)bucketsCapacity);
                bucketsCapacity = cap > int.MaxValue ? int.MaxValue : (int)cap;        //int.MaxValue is prime
            }
            buckets = new LinkedList<T>[bucketsCapacity];
            
            // add collection elements to hashtable
            foreach (var item in collection)
                this.Add(item);
        }

        public LinkedList<T>[] buckets;
        private EqualityComparer<T> equalityComparer;
        private float maxLoadFactor = 1.2F;
        private int bucketsCapacity;
        private int bucketsCount;
        private int elementsCount;

        /// <summary>
        /// Gets the count of elements stored in hashtable
        /// </summary>
        public int Count
        {
            get { return this.elementsCount; }
        }
        /// <summary>
        /// Gets the current count of buckets. This is less than or equal to buckets capacity.
        /// </summary>
        public int BucketsCount
        {
            get { return this.bucketsCount; }
        }
        /// <summary>
        /// Gets the capacity of buckets (the max number of buckets hashtable can hold without reallocation). 
        /// </summary>
        public int BucketsCapacity
        {
            get { return this.bucketsCapacity; }
        }
        /// <summary>
        /// Gets/sets the maximum load factor. If load factor exceeds this value, buckets collection reallocates itself  at the next call to Add method.
        /// When set, the value is rounded to two decimal places (1.231 -> 1.23). The default is 1.2.
        /// </summary>
        /// <remarks>
        /// Smaller load factors cause faster average lookup times at the cost of increased memory consumption.
        /// </remarks>
        public float MaxLoadFactor
        {
            get { return this.maxLoadFactor; }
            set
            {
                if (value < 0.1) throw new ArgumentException(string.Format("Max load factor must be at least 0.1"));
                maxLoadFactor = (float)Math.Round(value, 2);
            }
        }
        /// <summary>
        /// The load factor equals to the ratio of elements count to buckets capacity. The value is rounded to two decimal places (e.g. 1.231 -> 1.23).
        /// </summary>
        public float LoadFactor
        {
            get
            {
                if (bucketsCount == 0 || elementsCount == 0) return 0;        //no element is added yet
                return (float)Math.Round((double)elementsCount / (double)bucketsCapacity, 2);
            }
        }       
        #endregion

        #region "Methods and interfaces"

        /// <summary>
        /// Empties the collection without changing the capacity and the count of buckets.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
                if (buckets[i] != null) buckets[i].Clear();
            elementsCount = 0;
        }
        /// <summary>
        /// Returns true if collection contains the specified object and false otherwise. Worst case complexity is O(Count/BucketsCount).
        /// </summary>
        public bool Contains(T value)
        {
            int bucketIndex = equalityComparer.GetHashCode(value) % bucketsCapacity;    //if exists, it must be in this bucket
            if (buckets[bucketIndex] == null) return false;
            return buckets[bucketIndex].Contains(value);                                //search is linear in linked list which might contain max Count/bucketCount elements.
        }
        /// <summary>
        /// Copies elements of hashtable to specified array starting from given index in array (shallow copy)
        /// </summary>
        public void CopyTo(T[] array, int startingIndex)
        {
            if (array.Length - startingIndex < Count) throw new InvalidOperationException("Not enough space in array");
            int index = 0;
            foreach (var list in buckets)
            {
                if (list != null)
                    foreach (T value in list)
                    { 
                        array[startingIndex + index] = value;
                        index++;
                    }
            }
        }
        /// <summary>
        /// Removes the first occurance of specified value. Returns true if the value was removed, false otherwise. Worst case complexity is O(Count/BucketsCount).
        /// Does not cause reallocation. If you want to decrease the size of hashtable after removal of many items call DecreaseCapacity explicitly.
        /// </summary>
        public bool Remove(T value)
        {
            int bucketIndex = equalityComparer.GetHashCode(value) % bucketsCapacity;    //if exists it must be in this bucket
            if (buckets[bucketIndex] == null) return false;     //not found
            var removed = buckets[bucketIndex].Remove(value);
            if (removed) elementsCount--;
            return removed;
        }
        /// <summary>
        /// Adds new item to hashtable. Might cause a reallocation.
        /// </summary>
        public void Add(T item)
        {
            int hashCode = equalityComparer.GetHashCode(item);
            int bucketIndex = hashCode % bucketsCapacity;
            if (buckets[bucketIndex] == null)
            {
                buckets[bucketIndex] = new LinkedList<T>();
                bucketsCount++;
            }
            buckets[bucketIndex].Add(item);
            elementsCount++;
            
            //-------------------------- reallocate the buckets collection if required
            // get new bucket capacity if required
            if (LoadFactor <= MaxLoadFactor) return;
            ulong newBucketsCapacity = 17;
            if (bucketsCapacity < int.MaxValue / 2)
            { 
                newBucketsCapacity = (ulong) bucketsCapacity * 2;
                newBucketsCapacity = NumericAlgorithms.FindNextPrime((ulong)newBucketsCapacity);
            }
            else
                newBucketsCapacity = int.MaxValue;  //its prime

            // for each value in hash table get new index in new reallocated array, copy that value and count new buckets 
            var newBuckets = new LinkedList<T>[newBucketsCapacity];
            int newBucketsCount = 0;
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    var list = buckets[i];
                    if (list.Count != 0)
                    {
                        foreach (T value in list)
                        {
                            hashCode = equalityComparer.GetHashCode(value);
                            int newBucketIndex = hashCode % (int)newBucketsCapacity;
                            if (newBuckets[newBucketIndex] == null)
                            {
                                newBuckets[newBucketIndex] = new LinkedList<T>();
                                newBucketsCount++;
                            }
                            newBuckets[newBucketIndex].Add(value);
                        }
                    }
                }
            }

            // copy new values
            buckets = newBuckets;
            bucketsCapacity = (int)newBucketsCapacity;
            bucketsCount = newBucketsCount;
        }
        /// <summary>
        /// Reallocates hashtable and changes the capacity of buckets so that actual load factor approaches to maximum load factor. 
        /// Call this function if you have changed the max load factor or want to decrease the size of hashtable after removing many items from it.
        /// </summary>
        public void Reallocate()
        {
            ulong newCapacity = (ulong)(elementsCount / maxLoadFactor);
            if (newCapacity > int.MaxValue) newCapacity = int.MaxValue;     // it is prime
            if (newCapacity == 0) newCapacity = 17;                         // no elements in array
            int newBucketsCapacity = (int)newCapacity;
            if (newBucketsCapacity >= BucketsCapacity) return;              // no need to reallocate
            var newBuckets = new LinkedList<T>[newBucketsCapacity];
            int newBucketsCount = 0;
            
            // for each value in hash table get new index in new reallocated array, copy that value and count new buckets count
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    var list = buckets[i];
                    if (list.Count != 0)
                    {
                        foreach (T value in list)
                        {
                            int hashCode = equalityComparer.GetHashCode(value);
                            int newBucketIndex = hashCode % newBucketsCapacity;
                            if (newBuckets[newBucketIndex] == null)
                            {
                                newBuckets[newBucketIndex] = new LinkedList<T>();
                                newBucketsCount++;
                            }
                            newBuckets[newBucketIndex].Add(value);
                        }
                    }
                }
            }

            // copy new values
            buckets = newBuckets;
            bucketsCapacity = newBucketsCapacity;
            bucketsCount = newBucketsCount;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    sb.Append(buckets[i].ToString());
                    sb.Append(", ");
                }
            }
            var result = sb.ToString();
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result.Substring(0, result.Length - 2);
        }
        #endregion
    }
}
