using System; 

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents doubly linked list
    /// </summary>
    public sealed class LinkedList<T> : System.Collections.Generic.ICollection<T>,
                                        System.Collections.Generic.IEnumerable<T>, 
                                        System.Collections.IEnumerable
    {
        #region "Constructors, Properties and fields"
        public LinkedList() { }
        public LinkedList(System.Collections.Generic.IEnumerable<T> collection)
        {
            foreach (T item in collection)
                this.AddAfter(Last, item);
        }
        private LinkedListNode<T> first;
        private LinkedListNode<T> last;
        private int count;
        public LinkedListNode<T> First
        {
            get { return this.first; }
        }
        public LinkedListNode<T> Last
        {
            get { return this.last; }
        }
        public int Count
        {
            get { return this.count; }
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Inserts new node at the left
        /// </summary>
        public void AddLeft(T value)
        {
            this.AddLeft(new LinkedListNode<T>(value));
        }
        /// <summary>
        /// Inserts new node at the left
        /// </summary>
        public void AddLeft(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("Node is null");
            if (node.Previous != null) throw new InvalidOperationException("Previous link of the first node must be null");

            if (count == 0)
            {
                node.Next = null;
                first = node;
                last = node;
            }
            else
            {
                first.Previous = node;
                node.Next = first;
                first = node;
            }
            node.LinkedList = this;
            count++;
        }
        /// <summary>
        /// Inserts new node at the right
        /// </summary>
        public void AddRight(T value)
        {
            AddRight(new LinkedListNode<T>(value));
        }
        /// <summary>
        /// Inserts new node at the right
        /// </summary>
        public void AddRight(LinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("Node is null");
            if (node.Next != null) throw new InvalidOperationException("Next link of the last node must be null");
            if (count == 0)
            {
                node.Previous = null;
                last = node;
                first = node;
            }
            else
            {
                last.Next = node;
                node.Previous = last;
                last = node;
            }
            node.LinkedList = this;
            count++;
        }
        /// <summary>
        /// Inserts new node after the specified node
        /// </summary>
        public void AddAfter(LinkedListNode<T> existingNode, T value)
        {
            AddAfter(existingNode, new LinkedListNode<T>(value));
        }
        /// <summary>
        /// Inserts new node after the specified node
        /// </summary>
        public void AddAfter(LinkedListNode<T> existingNode, LinkedListNode<T> newNode)
        {
            if (count == 0) throw new InvalidOperationException("There are no items in list.");
            var current = first;
            bool found = false;
            while (current != null)
            {
                if (existingNode == current)
                {
                    found = true;
                    if (existingNode.Next != null) existingNode.Next.Previous = newNode;
                    newNode.Next = existingNode.Next;
                    newNode.Previous = existingNode;
                    existingNode.Next = newNode;
                    newNode.LinkedList = this;
                    count++;
                    break;
                }
                current = current.Next;
            }
            if (!found) throw new InvalidOperationException("Node does not exists");
        }
        /// <summary>
        /// Inserts new node before the specified node
        /// </summary>
        public void AddBefore(LinkedListNode<T> existingNode, T value)
        {
            AddBefore(existingNode, new LinkedListNode<T>(value));
        }
        /// <summary>
        /// Inserts new node before the specified node
        /// </summary>
        public void AddBefore(LinkedListNode<T> existingNode, LinkedListNode<T> newNode)
        {
            if (count == 0) new InvalidOperationException("There are no items in list.");
            var current = first;
            bool found = false;
            while (current != null)
            {
                if (existingNode == current)
                {
                    found = true;
                    if (existingNode.Previous != null) existingNode.Previous.Next = newNode;
                    newNode.Previous = existingNode.Previous;
                    newNode.Next = existingNode;
                    existingNode.Previous = newNode;
                    newNode.LinkedList = this;
                    count++;
                    break;
                }
                current = current.Next;
            }
            if (!found) throw new InvalidOperationException("Node does not exists");
        }
        /// <summary>
        /// Empties the linked list
        /// </summary>
        public void Clear()
        {
            first.Next = null;
            last.Previous = null;
            count = 0;
        }
        /// <summary>
        /// Returns true if the list contains specified value
        /// </summary>
        public bool Contains(T value)
        {
            var current = first;
            while (current != null)
            {
                if (current.Value.Equals(value)) return true;
                current = current.Next;
            }
            return false;
        }
        /// <summary>
        /// Deletes the first occurance of the node containing the specified value
        /// </summary>
        /// <returns>True if specified value was removed, false otherwise</returns>
        public bool Remove(T value)
        {
            var current = first;
            while (current != null)
            {
                if (current.Value.Equals(value))
                {
                    Remove(current);
                    return true;
                }
                current = current.Next;
            }
            return false;
        }
        /// <summary>
        /// Deletes the specified node
        /// </summary>
        public void Remove(LinkedListNode<T> node)
        {
            if (node.LinkedList != this) throw new InvalidOperationException("The node does not belongs to the list");
            if (count == 1)
            {
                first = null;
                last = null;
            }
            else
            {
                if (node == first)
                {
                    first.Next.Previous = null;
                    first = first.Next;
                }
                else if (node == last)
                {
                    last.Previous.Next = null;
                    last = last.Previous;
                }
                else
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                }
            }
            node.LinkedList = null;
            count--;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (T item in this)
            {
                sb.Append(item.ToString());
                sb.Append(", ");
            }
            var result = sb.ToString();
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result.Substring(0,result.Length-2);
        }
        #endregion

        #region "interface implementations"
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var current = first;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            var current = first;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Adds new entry at the right
        /// </summary>
        public void Add(T value)
        {
            this.AddRight(value);
        }
        /// <summary>
        /// Copies to specified array starting from arrayIndex
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("Array can't be null");
            if (arrayIndex < 0 || arrayIndex > array.Length - 1) throw new InvalidOperationException("Array index is out of range");
            if (this.Count > array.Length - arrayIndex) throw new InvalidOperationException("No suffiecient place in array to copy nodes");
            var current = first;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }        
        #endregion
    }
    /// <summary>
    /// Represents single entry in LinkedList
    /// </summary>
    public class LinkedListNode<T>
    {
        public LinkedListNode(T value)
        {
            this.Value = value;
        }
        public T Value { get; set; }
        private LinkedList<T> list;
        private LinkedListNode<T> next;
        private LinkedListNode<T> previous;

        public LinkedListNode<T> Previous
        {
            get { return this.previous; }
            internal set { this.previous = value; }
        }
        public LinkedListNode<T> Next
        {
            get { return this.next; }
            internal set { this.next = value; }
        }
        public LinkedList<T> LinkedList
        {
            get { return this.list; }
            internal set { this.list = value; }
        }
    }
}
