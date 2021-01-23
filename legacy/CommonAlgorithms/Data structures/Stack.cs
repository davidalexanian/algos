using System;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents last in first out (LIFO) collection (based on double linked list)
    /// </summary>
    public sealed class Stack<T> : StackQueueBase<T>
    {
        public Stack(): base() {  }
        public Stack(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Returns the top element of the stack without removing it
        /// </summary>
        public T Peek()
        {
            if (list.Count == 0) throw new InvalidOperationException("Stack is empty");
            return list.Last.Value;
        }
        /// <summary>
        /// Removes and returns the top element of the stack
        /// </summary>
        public T Pop()
        {
            if (list.Count == 0) throw new InvalidOperationException("Stack is empty");
            T result = list.Last.Value;
            list.Remove(list.Last);
            return result;
        }
        /// <summary>
        /// Inserts new value at the top of the stack
        /// </summary>
        public void Push(T value)
        {
            list.AddRight(value);
        }
    }
}
