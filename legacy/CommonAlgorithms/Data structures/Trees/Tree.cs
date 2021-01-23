using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents a n-ary tree(node might have any number of children)
    /// </summary>
    public class Tree<T>
    {
        public Tree() {}
        public Tree(T rootValue) { Root = new TreeNode<T>(rootValue); }

        private EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        /// <summary>
        /// Equality comparer for type parameter
        /// </summary>
        public EqualityComparer<T> EqualityComparer
        {
            get { return equalityComparer;}
            set
            {
                if (value == null) throw new ArgumentNullException("EqualityComparer");
                equalityComparer = value;
            }
        }

        private TreeNode<T> root;
        /// <summary>
        /// Gets/sets tree's root
        /// </summary>
        public TreeNode<T> Root
        {
            get { return root; }
            set
            {
                if (value != null && value.Parent != null) throw new InvalidOperationException("Root can't have parent node");
                root = value;
            }
        }
        /// <summary>
        /// Clears the tree by setting root to null
        /// </summary>
        public virtual void Clear()
        {
            root = null;
        }
        /// <summary>
        /// Returns true if the tree contains a node with specified value and false otherwise. 
        /// </summary>
        public bool Contains(T value)
        {
            return Root.Contains(value,equalityComparer);
        }
        /// <summary>
        /// Removes the first occurance of node containing the specified value and all its children.
        /// </summary>
        public bool Remove(T value)
        {
            TreeNode<T> node = FindNodeByValue(value);
            if (node == null) return false;
            Remove(node);
            return true;
        }
        /// <summary>
        /// Removes specified node and all its children from the tree
        /// </summary>
        public void Remove(TreeNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (node.Parent == null)
                root = null;
            else
                node.Parent.Children.Remove(node);
        }
        /// <summary>
        /// Returns the first occurance of node containing the specified value and null if search was not successfull.
        /// </summary>
        private TreeNode<T> FindNodeByValue(T value)
        {
            return Root.FindNodeByValue(value, equalityComparer);
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (Root == null) yield break;

            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this.Root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                yield return node.Value;
                foreach (var item in node.Children)
                    queue.Enqueue(item);
            }
        }
    }

    /// <summary>
    /// Represents a tree node that can contain many children
    /// </summary>
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        public TreeNode()
        {
            children = new TreeNodeList(this);
        }
        public TreeNode(T value)
        {
            this.Value = value;
            children = new TreeNodeList(this);
        }
        
        private TreeNode<T> parent = null;       
        private TreeNodeList children;

        /// <summary>
        /// Value of the node
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Parent of given node
        /// </summary>
        public TreeNode<T> Parent { get { return parent; } }

        /// <summary>
        /// Children of given node
        /// </summary>
        public TreeNodeList Children
        {
            get { return children; }
        }
        /// <summary>
        /// Adds specified value as direct child. Returns added node.
        /// </summary>        
        public TreeNode<T> AddChild(T value)
        {
            var tn = new TreeNode<T>(value);
            return this.children.Add(tn);
        }
        /// <summary>
        /// Adds specified node as direct child. Returns added node.
        /// </summary>        
        public TreeNode<T> AddChild(TreeNode<T> node)
        {
            return this.children.Add(node);
        }
        /// <summary>
        /// True if node is leaf, false otherwise
        /// </summary>
        public bool IsLeaf
        {
            get { return Children.Count == 0; }
        }
        /// <summary>
        /// Returns the depth/level of the node
        /// </summary>
        public int Level
        {
            get {
                int i = 0;
                var node = this;
                while (node != null)
                {
                    node = node.Parent;
                    i++;
                }
                return i;
            }
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
        /// <summary>
        /// Returns the first occurance of node containing the specified value and null if search was not successfull.
        /// </summary>
        public TreeNode<T> FindNodeByValue(T value)
        {
            return FindNodeByValue(value, EqualityComparer<T>.Default);
        }
        /// <summary>
        /// Returns the first occurance of node containing the specified value and null if search was not successfull.
        /// </summary>
        public TreeNode<T> FindNodeByValue(T value, EqualityComparer<T> comparer)
        {
            foreach (var node in this)
                if (comparer.Equals(node.Value, value))
                    return node;
            return null;
        }
        /// <summary>
        /// Depth-first enumeration.
        /// </summary>
        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                yield return node;
                foreach (var item in node.Children)
                    queue.Enqueue(item);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Returns true if a node with specified value is found somewhere down the given node and false otherwise.
        /// </summary>
        public bool Contains(T value, EqualityComparer<T> comparer)
        {
            return this.FindNodeByValue(value, comparer) != null;
        }
        /// <summary>
        /// Returns true if specified node is child of current instance
        /// </summary>
        public bool IsChild(TreeNode<T> node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return false;
        }
        /// <summary>
        /// Represents collection of tree nodes.
        /// </summary>
        public class TreeNodeList
        {
            public TreeNodeList(TreeNode<T> tn)
            {
                if (tn == null) throw new ArgumentNullException("tn");
                owner = tn;
                nodes = new List<TreeNode<T>>();                
            }
            public TreeNodeList(int capacity, TreeNode<T> tn)
            {
                if (tn == null) throw new ArgumentNullException("tn");
                owner = tn;
                nodes = new List<TreeNode<T>>(capacity);
            }

            private TreeNode<T> owner;
            private List<TreeNode<T>> nodes;
            
            /// <summary>
            /// Children count
            /// </summary>
            public int Count
            {
                get { return nodes.Count; }
            }
            /// <summary>
            /// Adds new direct child to current instance
            /// </summary>
            public TreeNode<T> Add(TreeNode<T> node)
            {
                if (node == null) throw new ArgumentNullException("node");
                if (node == owner) throw new InvalidOperationException("Node can't be added to its own children list");
                node.parent = owner;                
                nodes.Add(node);
                return node;
            }
            /// <summary>
            /// Removes and returns specified node and sets node.Parent to null. Will raise error if node is not direct child of current instance.
            /// </summary>
            public void Remove(TreeNode<T> node)
            {
                if (node.Parent != owner) throw new InvalidOperationException("Node is not a child of specified node");
                node.parent = null;
                if (!nodes.Remove(node)) throw new InvalidOperationException("Not was not found");
            }
            /// <summary>
            /// Removes all childs
            /// </summary>
            public void Clear()
            {
                nodes.Clear();
            }            
            public override string ToString()
            {
                return nodes.ToString();
            }
            public IEnumerator<TreeNode<T>> GetEnumerator()
            {
                return nodes.GetEnumerator();
            }
            public TreeNode<T> this[int index]
            {
                get { return nodes[index]; }
                set { nodes[index] = value; }
            }
        }
    }
}
