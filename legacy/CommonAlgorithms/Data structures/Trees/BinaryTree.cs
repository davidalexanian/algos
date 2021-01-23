using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents not sorted binary tree (a tree where a node might have max 2 children). 
    /// </summary>
    public class BinaryTree<T> : IEnumerable<T>
    {
        private BinaryTreeNode<T> root;
        public BinaryTreeNode<T> Root
        {
            get { return root; }
            set {
                if (value != null && value.Parent != null) throw new InvalidOperationException("Root node can't have parent");
                root = value;
            }
        }
        /// <summary>
        /// Empties the tree
        /// </summary>
        public void Clear() { root = null; }
        /// <summary>
        /// Returns true if a node with specified value is found and false otherwise.
        /// </summary>
        public bool Contains(T value)
        {
            return root.Contains(value);
        }
        /// <summary>
        /// Removes first occurance of a node containing specified value. Returns true if a node was removed and false otherwise.
        /// </summary>
        public bool Remove(T value)
        {
            if (root == null) return false;
            if (root.EqualityComparer.Equals(root.Value, value))
            {
                Clear();
                return true;
            }
            return root.RemoveChildNodeByValue(value);
        }
        /// <summary>
        /// Removes specifed child node.
        /// </summary>
        public void RemoveNode(BinaryTreeNode<T> node)
        {
            if (node == root)
                Clear();
            else
                root.RemoveChildNode(node);
        }
        /// <summary>
        /// Returns a node with specified value if one found and null otherwise.
        /// </summary>        
        public BinaryTreeNode<T> FindNodeByValue(T value)
        {
            if (root == null) return null;
            return root.FindNodeByValue(value);
        }
        /// <summary>
        /// Depth-first iteration
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (root == null) yield break;
            foreach (var item in root)
                yield return item.Value;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #region "tree visitors/traversal"
        /// <summary>
        /// Preorder traversal
        /// </summary>
        /// <param name="node">Root node to start traversal from. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitPreorder(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node != null)
            {
                action(node.Value);     //process the node
                if (node.Left != null) VisitPreorder(node.Left, action);    //process left
                if (node.Right != null) VisitPreorder(node.Right, action);  //process right
            }
        }
        /// <summary>
        /// Inorder traversal
        /// </summary>
        /// <param name="node">Root node to start traversal. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitInorder(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node != null)
            {
                if (node.Left != null) VisitInorder(node.Left, action);
                action(node.Value);
                if (node.Right != null) VisitInorder(node.Right, action);
            }
        }
        /// <summary>
        /// Postorder traversal
        /// </summary>
        /// <param name="node">Root node to start traversal. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitPostorder(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node != null)
            {
                if (node.Left != null) VisitPostorder(node.Left, action);
                if (node.Right != null) VisitPostorder(node.Right, action);
                action(node.Value);
            }
        }
        /// <summary>
        /// Depth-first traversal (level by level)
        /// </summary>
        /// <param name="node">Root node to start traversal. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitDepthFirst(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                action(n.Value);
                if (n.Left != null) queue.Enqueue(n.Left);
                if (n.Right != null) queue.Enqueue(n.Right);
            }
        }
        #endregion
    }
    /// <summary>
    /// Represents binary tree node (a node having max 2 children)
    /// </summary>
    public class BinaryTreeNode<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public T Value { get; set; }
        private BinaryTreeNode<T> left;
        private BinaryTreeNode<T> right;
        private BinaryTreeNode<T> parent;

        /// <summary>
        /// Left node of current node
        /// </summary>
        public BinaryTreeNode<T> Left
        {
            get { return left; }
            set
            {
                if (this.left != null) this.left.parent = null;     // detach the old left element if there is one
                if (value != null) value.parent = this;             // set new nodes's parent to this element
                this.left = value;
            }
        }
        /// <summary>
        /// Right node of current node
        /// </summary>
        public BinaryTreeNode<T> Right
        {
            get { return right; }
            set
            {
                if (this.right != null) this.right.parent = null;   // detach the old right element if there is one
                if (value != null) value.parent = this;             // set new nodes's parent to this element
                this.right = value;
            }
        }
        /// <summary>
        /// Gets the parent of current node. Setting this to null will detach the node from tree. Setting this to another node will move it to another node.
        /// </summary>
        public BinaryTreeNode<T> Parent
        {
            get { return parent; }
        }
        private EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        /// <summary>
        /// Equality comparer of T
        /// </summary>
        public EqualityComparer<T> EqualityComparer
        {
            get { return equalityComparer; }
            set
            {
                if (value == null) throw new ArgumentNullException("EqualityComparer");
                equalityComparer = value;
            }
        }
        public BinaryTreeNode() { }
        public BinaryTreeNode(T value) { this.Value = value; }

        /// <summary>
        /// Returns true if current node is leaf, and false otherwise
        /// </summary>
        public bool IsLeaf
        {
            get { return left == null && right == null; }
        }
        /// <summary>
        /// Returns the depth/level of the node
        /// </summary>
        public int Level
        {
            get
            {
                int level = 0;
                var node = this;
                while (node != null)
                {
                    level++;
                    node = node.parent;
                }
                return level;
            }
        }
        /// <summary>
        /// Height of current node (length of longest path from this node down to leaf). Complexity is exponential.
        /// </summary>
        public int Height
        {
            get
            {
                if (this.IsLeaf) return 0;
                int heightLeft = this.Left == null ? 0 : this.Left.Height;
                int heightRight = this.Right == null ? 0 : this.Right.Height;
                return System.Math.Max(heightLeft, heightRight) + 1;
            }
        }
        /// <summary>
        /// Returns true if specified node is child of current instance (not necessarily a direct child)
        /// </summary>
        public bool IsChild(BinaryTreeNode<T> node)
        {
            while (node.Parent != null)
            {
                if (node.parent == this) return true;
                node = node.Parent;
            }
            return false;
        }
        /// <summary>
        /// Returns true if a node with specified value is found somewhere down the given node and false otherwise.
        /// </summary>
        public bool Contains(T value)
        {
            foreach (var node in this)
                if (equalityComparer.Equals(node.Value, value))
                    return true;
            return false;
        }
        /// <summary>
        /// Removes first occurance of child node(not necessarily direct) containing specified value. Returns true if a node was removed and false otherwise.
        /// </summary>
        public bool RemoveChildNodeByValue(T value)
        {
            if (this.left != null)
            {
                var node = this.left.FindNodeByValue(value);
                if (node != null)
                {
                    RemoveChildNode(node);
                    return true;
                }
            }
            if (this.right != null)
            {
                var node = this.right.FindNodeByValue(value);
                if (node != null)
                {
                    RemoveChildNode(node);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Removes specifed child node.
        /// </summary>        
        public void RemoveChildNode(BinaryTreeNode<T> node)
        {
            if (!this.IsChild(node)) throw new InvalidOperationException("Specified node is not a child node of current instance");
            if (node.Parent != null)
            {
                // detach node from its parent Left/Right references
                if (node.parent.Left == node) node.parent.Left = null;
                if (node.parent.Right == node) node.parent.Right = null;
                node.parent = null;
            }
        }
        /// <summary>
        /// Returns the first occurance of node down current instace(including) containing specified value.
        /// </summary>
        public BinaryTreeNode<T> FindNodeByValue(T value)
        {
            foreach (var node in this)
                if (equalityComparer.Equals(node.Value, value))
                    return node;
            return null;
        }
        public override string ToString()
        {
            return Value.ToString();
        } 
        /// <summary>
        /// Depth-first enumerator
        /// </summary>
        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                yield return node;
                if(node.Left != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
