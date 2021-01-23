using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Base class for stacks and queues (based on double linked list)
    /// </summary>
    public abstract class StackQueueBase<T> : System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
    {
        #region "Constructors, Properties and fields"
        public StackQueueBase() { list = new LinkedList<T>(); }
        public StackQueueBase(System.Collections.Generic.IEnumerable<T> collection) { list = new LinkedList<T>(collection); }
        protected LinkedList<T> list;
        public int Count { get { return list.Count; } }
        #endregion

        #region "Methods and interfaces"
        public void Clear()
        {
            list.Clear();
        }
        public bool Contains(T value)
        {
            return list.Contains(value);
        }        
        public override string ToString()
        {
            return list.ToString();
        }
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return ((System.Collections.Generic.IEnumerable<T>)list).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)list).GetEnumerator();
        }
        #endregion
    }
}
