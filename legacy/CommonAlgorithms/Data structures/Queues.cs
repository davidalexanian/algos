using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents first in first out (FIFO) collection (based on double linked list)
    /// </summary>
    public class Queue<T> : StackQueueBase<T>
    {
        public Queue() : base() { }
        public Queue(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Returns the element at the beginning of the queue without removing it
        /// </summary>
        public T Peek()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            return list.First.Value;
        }
        /// <summary>
        /// Removes and returns an element at the beginning of the queue
        /// </summary>
        public T Dequeue()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            T result = list.First.Value;
            list.Remove(list.First);
            return result;
        }
        /// <summary>
        /// Adds new entry at the end of the queue
        /// </summary>
        public void Enqueue(T value)
        {
            list.AddRight(value);
        }
    }
    /// <summary>
    /// Represents double-ended queue, where elements can be added/removed from both ends (based on linked list)
    /// </summary>
    public sealed class Dequeue<T> : StackQueueBase<T>
    {
        public Dequeue() : base() { }
        public Dequeue(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Removes and returns an element at the beginning of the queue
        /// </summary>
        public T DequeueFirst()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            T result = list.First.Value;
            list.Remove(list.First);
            return result;
        }
        /// <summary>
        /// Removes and returns an element at the end of the queue
        /// </summary>
        public T DequeueLast()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            T result = list.Last.Value;
            list.Remove(list.Last);
            return result;
        }
        /// <summary>
        /// Adds new entry at the beginning of the queue
        /// </summary>
        public void EnqueueFirst(T value)
        {
            list.AddLeft(value);
        }
        /// <summary>
        /// Adds new entry at the end of the queue
        /// </summary>
        public void EnqueueLast(T value)
        {
            list.AddRight(value);
        }
        /// <summary>
        /// Returns the element at the beginning of the queue without removing it
        /// </summary>
        public T PeekFirst()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            return list.First.Value;
        }
        /// <summary>
        /// Returns the element at the end of the queue without removing it
        /// </summary>
        public T PeekLast()
        {
            if (list.Count == 0) throw new InvalidOperationException("Queue is empty");
            return list.Last.Value;
            
        }
    }
}
