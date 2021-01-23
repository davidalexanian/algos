using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. 
    /// </summary>
    public class List<T> : System.Collections.Generic.IList<T>,
                           System.Collections.Generic.ICollection<T>,
                           System.Collections.Generic.IEnumerable<T>,
                           System.Collections.IEnumerable
    {
        #region "Constructors,properties and fields"
        /// <summary>
        /// Initializes the collection with initial capacity 4
        /// </summary>
        public List()
        {
            this.capacity = 4;
            innerArray = new T[capacity];
        }
        /// <summary>
        /// Initializes the collection with given capacity
        /// </summary>
        public List(int capacity)
        {
            if (capacity < 4) capacity = 4;
            this.capacity = capacity;
            innerArray = new T[capacity];
        }
        /// <summary>
        /// Initializes the list based on the given collection
        /// </summary>
        public List(IEnumerable<T> collection)
        {
            this.capacity = collection.Count();
            if (capacity < 4) capacity = 4;
            innerArray = new T[capacity];
        }

        protected T[] innerArray;
        protected int capacity;
        protected int count;

        /// <summary>
        /// Gets/sets the capacity of the collection
        /// </summary>
        public int Capacity
        {
            get { return this.capacity; }
            private set
            {
                if (value < count ) throw new InvalidOperationException("List contains more items then the specified capacity");
                var newArray = new T[value];
                for (int i = 0; i < count; i++) newArray[i] = innerArray[i];
                this.innerArray = newArray;
                this.capacity = value;
            }
        }
        /// <summary>
        /// Count elements in the collection
        /// </summary>
        public int Count {
            get { return this.count; }
        }
        /// <summary>
        /// Returns false.
        /// </summary>
        public bool IsReadOnly {
            get { return false; }
        }
        public T this[int index]
        {
            get
            {
                if (index >= count || index < 0) throw new ArgumentException("Index is out of range");
                return innerArray[index];
            }
            set
            {
                if (index >= count || index < 0) throw new ArgumentException("Index is out of range");
                innerArray[index] = value;
            }
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Adds new entry to the collection
        /// </summary>
        public virtual void Add(T value)
        {
            // resize before inserting if required
            if (count == int.MaxValue) throw new Exception("The max size of the list is reached.");
            if (count == capacity)
            {
                if (Capacity >= int.MaxValue / 2)
                    Capacity = int.MaxValue;
                else
                    Capacity = 2 * Capacity;
            }
            this.innerArray[count++] = value;
        }
        /// <summary>
        /// Adds range to the collections
        /// </summary>
        public virtual void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
                Add(item);
        }
        /// <summary>
        /// Removes all elements from collection without changing the capacity
        /// </summary>
        public virtual void Clear()
        {
            count = 0;
            innerArray = new T[capacity];
        }
        /// <summary>
        /// Returns true if collection contains given argument using T.Equals
        /// </summary>
        public virtual bool Contains(T value)
        {
            for (int i = 0; i < count; i++)
                if (innerArray[i].Equals(value))
                    return true;
            return false;
        }
        /// <summary>
        /// Copies to specified array starting from arrayIndex
        /// </summary>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("Array can't be null");
            if (arrayIndex < 0 || arrayIndex > array.Length - 1) throw new InvalidOperationException("Array index is out of range");
            if (this.Count > array.Length - arrayIndex) throw new InvalidOperationException("No suffiecient place in array to copy elements");

            for (int i = 0; i < count; i++)
                array[arrayIndex++] = innerArray[i];
        }
        /// <summary>
        /// Returns the index of the first occurance of the given value (-1 if not found)
        /// </summary>
        public virtual int IndexOf(T value)
        {
            for (int i = 0; i < count; i++)
                if (innerArray[i].Equals(value))
                    return i;
            return -1;
        }
        /// <summary>
        /// Removes element with given index
        /// </summary>
        public virtual void RemoveAt(int index)
        {
            if (count == 0) throw new InvalidOperationException("Collection is empty");
            if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("Index is out of range");
            for (int i = index; i < count-1; i++)
                innerArray[i] = innerArray[i+1];
            count--;
        }
        /// <summary>
        /// Removes the first occurrence of a specific object.
        /// </summary>
        /// <returns>True if an element was removed, false otherise.</returns>
        public virtual bool Remove(T value)
        {
            int i = IndexOf(value);
            if (i == -1) return false;
            RemoveAt(i);
            return true;
        }
        /// <summary>
        /// Removes all occurances of elements with predicate resulting to true
        /// </summary>
        /// /// <returns>True if an element was removed, false otherise.</returns>
        public virtual void RemoveAll(Predicate<T> predicate)
        {
            for (int i = 0; i < count; i++)
                if (predicate.Invoke(innerArray[i]))
                {
                    RemoveAt(i);
                    i--;
                }
        }
        /// <summary>
        /// Inserts new item at the specified index. 
        /// </summary>
        public virtual void Insert(int index, T item)
        {
            if (index < 0 || index >= count) throw new ArgumentException("Index is out of range");
            T element = innerArray[index];

            // move elements to the right starting from index
            this.Add(default(T));   //increments the capacity if required
            for (int j = count - 1; j > index; j--)
                innerArray[j] = innerArray[j - 1];

            this[index] = item;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for(int i=0; i < count; i++)
            {
                sb.Append(innerArray[i].ToString());
                sb.Append(", ");
            }
            var result = sb.ToString();
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result.Substring(0, result.Length - 2);
        }
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return innerArray[i];
        }
        public virtual IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return innerArray[i];
        }
        #endregion
    }
}
