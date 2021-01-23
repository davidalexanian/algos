using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.DataStructures
{
    /// <summary>
    /// Represents an AVL tree (binary sorted tree where heights of node's both subtrees differs by max 1).  Lookup, insertion, and deletion all take O(log n) time in both average and worst cases.
    /// </summary>
    public class AVLTree<T> : IEnumerable<T>
    {
        private AVLTreeNode root;
        private int count;
        /// <summary>
        /// Comparer using to compare values stored in AVL tree node.
        /// </summary>
        public readonly Comparer<T> Comparer = Comparer<T>.Default;

        public AVLTree() { }
        public AVLTree(Comparer<T> comparer) { this.Comparer = comparer; }

        /// <summary>
        /// Inserts new value into tree
        /// </summary>
        public AVLTree<T> Add(T value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (root == null)
                root = new AVLTreeNode(value);
            else
                AVLTreeNode.AddInner(value, Comparer, root, null, ref root);
            count++;
            return this;
        }
        /// <summary>
        /// Removes the value from the tree. Returns false if the tree does not contain the specified value and true otherwise. Removed node is replaced with its left child.
        /// </summary>        
        public void Remove(T value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (root == null) return;
            if (AVLTreeNode.Remove(value, Comparer, root, null, ref root)) count--;
        }
        /// <summary>
        /// Returns true if the tree contains the provided value and false otherwise. Complexity is logarithmic.
        /// </summary>
        public bool Contains(T value)
        {
            if (root == null) return false;
            return root.Contains(value, Comparer);
        }
        /// <summary>
        /// Total count of nodes
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// Root node of the tree
        /// </summary>
        public AVLTreeNode Root
        {
            get { return root; }
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

        #region "Tree visitors"
        /// <summary>
        /// Preorder traversal (node, left, right)
        /// </summary>
        /// <param name="node">Root node to start traversal from. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitPreorder(AVLTreeNode node, Action<T> action)
        {
            if (node != null)
            {
                action(node.Value);     //process the node
                if (node.Left != null) VisitPreorder(node.Left, action);    //process left
                if (node.Right != null) VisitPreorder(node.Right, action);  //process right
            }
        }
        /// <summary>
        /// Inorder traversal (left, node, right). Printing with this traversal results in increasing ordered output of values.
        /// </summary>
        /// <param name="node">Root node to start traversal. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitInorder(AVLTreeNode node, Action<T> action)
        {
            if (node != null)
            {
                if (node.Left != null) VisitInorder(node.Left, action);
                action(node.Value);
                if (node.Right != null) VisitInorder(node.Right, action);
            }
        }
        /// <summary>
        /// Postorder traversal (left, right, node)
        /// </summary>
        /// <param name="node">Root node to start traversal. Use tree.Root to visit entire tree</param>
        /// <param name="action">Action delegate to execute for each node</param>
        public static void VisitPostorder(AVLTreeNode node, Action<T> action)
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
        public static void VisitDepthFirst(AVLTreeNode node, Action<T> action)
        {
            if (node == null) return;
            var queue = new Queue<AVLTreeNode>();
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

        #region "AVL tree node"
        /// <summary>
        /// Represents a node of AVL tree.
        /// </summary>
        /// <remarks>
        /// AVL tree nodes have following characteristics:
        ///     1. Given a node, its left node's value is less from it and right node's value is bigger or equal to the value of the node (sorted tree's property).
        ///     2. The lengths of left and right subtrees of given node differ maximum by one (tree is balanced)
        /// </remarks>
        public class AVLTreeNode : IEnumerable<AVLTreeNode>
        {
            private T value;
            private AVLTreeNode left;
            private AVLTreeNode right;
            private AVLTreeNode parent;
            private int height = 0;

            /// <summary>
            /// Balance factor of the node ([height of left subtree] - [height of right sub tree]). If this value drops down from -1 or 
            /// exceeds 1, the tree is unbalanced at given node. Based on this value left or right rotations are made on this node to balance it.
            /// </summary>
            public int BalanceFactor
            {
                get {
                    if (this.IsLeaf) return 0;
                    if (this.left == null && this.right == null) return 0;
                    if (this.left != null && this.right == null) return this.left.height + 1;
                    if (this.left == null && this.right != null) return 0 - this.right.height - 1;
                    return this.left.height - this.right.height;
                }
            }
            /// <summary>
            /// Gets the value of the node
            /// </summary>
            public T Value
            {
                get { return this.value; }
                private set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    this.value = value;
                }
            }
            /// <summary>
            /// Gets left node of current node
            /// </summary>
            public AVLTreeNode Left
            {
                get { return left; }
                private set
                {
                    if (this.left != null) this.left.parent = null;     // detach the old left element if there is one
                    if (value != null) value.parent = this;             // set new nodes's parent to this element
                    left = value;
                }
            }
            /// <summary>
            /// Gets right node of current node
            /// </summary>
            public AVLTreeNode Right
            {
                get { return right; }
                private set
                {
                    if (this.right != null) this.right.parent = null;   // detach the old left element if there is one
                    if (value != null) value.parent = this;             // set new nodes's parent to this element
                    right = value;
                }
            }
            /// <summary>
            /// Returns the parent of given node
            /// </summary>
            public AVLTreeNode Parent
            {
                get { return parent; }                
            }

            public AVLTreeNode(T value) { this.Value = value; }

            /// <summary>
            /// True if current node is leaf, false otherwise
            /// </summary>
            public bool IsLeaf
            {
                get { return left == null && right == null; }
            }
            /// <summary>
            /// Lenght of path from parent down to this node (complexity is logarithmic)
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
            /// Height of current node (length of longest path from this node down to leaf). Computing height this way has linear complexity (recursively visits each node in both subtrees). 
            /// Use Height property instead which returns stored height of node.
            /// </summary>
            public int HeightLinear
            {
                get
                {
                    if (this.IsLeaf) return 0;
                    int heightLeft = this.left == null ? 0 : this.left.HeightLinear;
                    int heightRight = this.right == null ? 0 : this.right.HeightLinear;
                    return System.Math.Max(heightLeft, heightRight) + 1;
                }
            }
            /// <summary>
            /// Height of current node (length of longest path from this node down to leaf). 
            /// </summary>
            public int Height
            {
                get { return this.height; }
            }
            private static int ComputeHeight(AVLTreeNode node)
            {
                if (node == null) throw new ArgumentNullException("node");
                if (node.IsLeaf) return 0;
                int heightLeft = node.left == null ? 0 : node.left.height;
                int heightRight = node.right == null ? 0 : node.right.height;
                return Math.Max(heightLeft, heightRight) + 1;
            }
            /// <summary>
            /// Returns true if specified node is child of current node 
            /// </summary>
            public bool IsChild(AVLTreeNode node)
            {
                if (node == null) return false;
                var curNode = node;
                while (curNode.Parent != null)
                {
                    if (curNode.parent == this) return true;
                    curNode = node.Parent;
                }
                return false;
            }
            internal static bool Remove(T value, Comparer<T> comparer, AVLTreeNode node, AVLTreeNode parent, ref AVLTreeNode treeRoot)
            {
                if (node == null) return false;     //value was not found
                
                int compResult = comparer.Compare(value, node.value);
                if (compResult == 0)
                {
                    // we found a node to delete
                    if (node.IsLeaf)
                    {
                        DeleteLeafNode(node, ref treeRoot);
                    }
                    else if ((node.left == null && node.right != null) || node.left != null && node.right == null)
                    {
                        //node has one child which is leaf (otherwise node would be disbalanced). Swap node's value with child's value and delete child
                        var childNode = node.left == null ? node.right : node.left;
                        node.value = childNode.value;
                        DeleteLeafNode(childNode, ref treeRoot);
                    }
                    else
                    {
                        // node has left and right childs. Based on which one of node's subtrees is longer (to reduce the chance of rebalancing), 
                        // choose biggest node in left or smallest node in the right subtree. Biggest from left subtree does not have right child and smallest 
                        // from right subtree, left child (otherwise they won't be biggest and smallest nodes).
                        AVLTreeNode leafNode = null;
                        if (node.BalanceFactor >= 0)
                        {
                            leafNode = node.left;   //recursively go down by right branch of left subtree
                            while (leafNode.right != null) { leafNode = leafNode.right; }
                        }
                        else
                        {
                            leafNode = node.right;
                            while (leafNode.left != null) { leafNode = leafNode.left; }
                        }
                        // swap node's value with leafNode.Value and delete leafNode
                        node.value = leafNode.value;
                        DeleteLeafNode(leafNode, ref treeRoot);
                    }

                    return true;
                }
                else
                {
                    //recursively try left/right node
                    return Remove(value, comparer, (compResult==-1 ? node.left : node.right), node, ref treeRoot);   
                }
            }
            /// <summary>
            /// Removes leaf node from tree (all deletion from tree can be reduced to removing leaf node). 
            /// </summary>
            private static void DeleteLeafNode(AVLTreeNode node, ref AVLTreeNode rootNode)
            {
                if (! node.IsLeaf) throw new ArgumentException("Node should be a leaf");
                if (node.parent != null)
                {
                    // detach from parent
                    if (node.parent.left == node) node.parent.left = null;
                    if (node.parent.right == node) node.parent.right = null;
                }
                else
                {
                    rootNode = null;
                    return;
                }
                var parentNode = node.parent;
                node.parent = null;

                // recompute heights of all parent nodes up to the root. Because node was removed they might be out of balance. 
                while (parentNode != null)
                {
                    parentNode.height = ComputeHeight(parentNode);
                    BalanceNode(parentNode, ref rootNode);
                    parentNode = parentNode.parent;
                }
            }
            /// <summary>
            /// Recursively tries nodes to find appropriate place for inserting and adds the value as new leaf node.
            /// </summary>
            /// <param name="value">Value of the node beeing inserted</param>
            /// <param name="comparer">Comparer used for comparisions</param>
            /// <param name="node">Current node beeing examined</param>
            /// <param name="parent">The parent of node</param>
            internal static void AddInner(T value, Comparer<T> comparer, AVLTreeNode node, AVLTreeNode parent, ref AVLTreeNode treeRoot)
            {
                if (node == null)
                {
                    // insert new leaf here
                    if (comparer.Compare(value, parent.Value) < 0)
                        parent.Left = new AVLTreeNode(value);
                    else
                        parent.Right = new AVLTreeNode(value);
                }
                else
                {
                    // recursively try nodes based on their values. As the recursion unwinds, we update the height and balance factor of each node
                    if (comparer.Compare(value, node.Value) < 0)
                        AddInner(value, comparer, node.left, node, ref treeRoot);   //small value goes to the left
                    else
                        AddInner(value, comparer, node.right, node, ref treeRoot);  //otherwise right

                    node.height = ComputeHeight(node);      //update height of node as it has new child (height might change)
                    BalanceNode(node, ref treeRoot);        // check and balance if required (it was in balance before insertion)
                }
            }
            /// <summary>
            /// Balances node if it is out of balance
            /// </summary>
            private static void BalanceNode(AVLTreeNode node, ref AVLTreeNode treeRoot)
            {
                if (node.BalanceFactor == 2)
                {
                    if (node.left.BalanceFactor == 1 || node.left.BalanceFactor == 0)   // second predicate is required only for removing only
                    {
                        RotateLeftLeft(node, ref treeRoot);     // left-left case                                
                        return;                                 // after rotation node changes its position in the tree
                    }
                    if (node.left.BalanceFactor == -1)
                    {
                        RotateLeftRight(node, ref treeRoot);    // left-right case                                
                        return;
                    }
                }
                else if (node.BalanceFactor == -2)
                {
                    if (node.right.BalanceFactor == 1)
                    {
                        RotateRightLeft(node, ref treeRoot);    // right-left case
                        return;
                    }
                    if (node.right.BalanceFactor == -1 || node.right.BalanceFactor == 0)    // second predicate is required only for removing only
                    {
                        RotateRightRight(node, ref treeRoot);   // right-right case
                        return;
                    }
                }
            }
            /// <summary>
            /// Left-left rotation of given node (inserted node is the left child of the left node of given node). We rotate node.Left to the right.
            /// </summary>            
            private static void RotateLeftLeft(AVLTreeNode node, ref AVLTreeNode treeRoot)
            {
                if (node.left == null) return;      //though should not occur
                var leftNode = node.left;
                var parentNode = node.parent;

                // detach node from its parent
                bool nodeIsLeftChildOfParent = false;
                if (parentNode != null)
                {
                    nodeIsLeftChildOfParent = parentNode.left == node;
                    if (nodeIsLeftChildOfParent)
                        parentNode.left = null;
                    else
                        parentNode.right = null;
                }
                node.parent = null;

                // detach leftNode from node
                leftNode.parent = null;
                node.left = null;

                // detach leftNode.Right from leftNode and attach it to node.Left
                node.left = leftNode.right;
                if (leftNode.right != null) leftNode.right.parent = node;
                leftNode.right = null;

                // attach node to leftNode.Right
                leftNode.right = node;
                node.parent = leftNode;

                // attach leftNode to parentNode as a left/right child
                if (parentNode != null)
                {
                    leftNode.parent = parentNode;
                    if (nodeIsLeftChildOfParent)
                        parentNode.left = leftNode;
                    else
                        parentNode.right = leftNode;
                }
                else
                    treeRoot = leftNode;

                // after rotation we need to recalculate heigths of node and its new parent
                node.height = ComputeHeight(node);      
                node.parent.height = ComputeHeight(node.parent);
            }
            /// <summary>
            /// Right-right rotation of given node (inserted node is in the right child of the right node of given node). We rotate node.right to the left.
            /// </summary>
            private static void RotateRightRight(AVLTreeNode node, ref AVLTreeNode treeRoot)
            {
                if (node.right == null) return;     //though should not occur
                var rightNode = node.right;
                var parentNode = node.parent;

                // detach node from its parent
                bool nodeIsLeftChildOfParent = false;
                if (parentNode != null)
                {
                    nodeIsLeftChildOfParent = parentNode.left == node;
                    if (nodeIsLeftChildOfParent)
                        parentNode.left = null;
                    else
                        parentNode.right = null;
                }
                node.parent = null;

                // detach rightNode from node
                rightNode.parent = null;
                node.right = null;

                // detach rightNode.left from rightNode and attach it to node.Right
                node.right = rightNode.left;
                if (rightNode.left != null) rightNode.left.parent = node;
                rightNode.left = null;

                // attach node to rightNode.left
                rightNode.left = node;
                node.parent = rightNode;

                // attach rightNode to parentNode as a left/right child
                if (parentNode != null)
                {
                    rightNode.parent = parentNode;
                    if (nodeIsLeftChildOfParent)
                        parentNode.left = rightNode;
                    else
                        parentNode.right = rightNode;
                }
                else
                    treeRoot = rightNode;

                // after rotation we need to recalculate heigths of node and its new parent
                node.height = ComputeHeight(node);
                node.parent.height = ComputeHeight(node.parent);
            }
            /// <summary>
            /// Left-right rotation of given node (inserted value is the right child of the left node of given node). 
            /// First we left rotate node.left.right (this becomes new node.left so that this case becomes left-left case), then right rotate new node.left
            /// </summary>
            private static void RotateLeftRight(AVLTreeNode node, ref AVLTreeNode treeRoot)
            {
                if (node.left == null || node.left.right == null) return;      //though should not occur

                //------------- left rotate node.left.right
                // detach node.left from node = leftNode
                var leftNode = node.left;
                node.left = null;
                leftNode.parent = null;

                // detach node.left.right from node.left = leftRightNode
                var leftRightNode = leftNode.right;
                leftNode.right = null;
                leftRightNode.parent = null;

                // attach leftRighNode to node.left
                node.left = leftRightNode;
                leftRightNode.parent = node;

                // attach leftNode to leftRightNode.Left
                leftRightNode.left = leftNode;
                leftNode.parent = leftRightNode;

                // recompute the heights
                leftNode.height = ComputeHeight(leftNode);
                leftRightNode.height = ComputeHeight(leftNode);
                node.height = ComputeHeight(node);

                //-------------- now we have left-left case for node.
                RotateLeftLeft(node, ref treeRoot);
            }
            /// <summary>
            /// Right-left rotation of node (inserted value is the left child of the right node of the given node).
            /// First we right rotate node.right.left and get right-right case for node then commit right-right rotation.
            /// </summary>
            private static void RotateRightLeft(AVLTreeNode node, ref AVLTreeNode treeRoot)
            {
                if (node.right == null || node.right.left == null) return;  //should not occur

                //------------- right rotate node.right.left
                // detach node.right from node = rightNode
                var rightNode = node.right;
                rightNode.parent = null;
                node.right = null;

                // detach rightNode.left from rightNode = rightLeftNode
                var rightLeftNode = rightNode.left;
                rightNode.left = null;
                rightLeftNode.parent = null;

                // attach rightLeftNode to node.right
                node.right = rightLeftNode;
                rightLeftNode.parent = node;

                // attach rightNode to rightLeftNode.right
                rightLeftNode.right = rightNode;
                rightNode.parent = rightLeftNode;

                // recompute heights
                rightNode.height = ComputeHeight(rightNode);
                rightLeftNode.height = ComputeHeight(rightLeftNode);
                node.height = ComputeHeight(node);

                //------------- now we have right-right case on the node
                RotateRightRight(node, ref treeRoot);
            }
            public override string ToString()
            {
                return Value.ToString();
            }
            /// <summary>
            /// Depth-first enumerator
            /// </summary>
            public IEnumerator<AVLTreeNode> GetEnumerator()
            {
                var queue = new Queue<AVLTreeNode>();
                queue.Enqueue(this);
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();
                    yield return node;
                    if (node.left != null) queue.Enqueue(node.left);
                    if (node.right != null) queue.Enqueue(node.right);
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            /// <summary>
            /// Recursively searches the provided value in the current instance and its children. Complexity is logarithmic.
            /// </summary>
            public bool Contains(T value, Comparer<T> comparer)
            {
                int compResult = comparer.Compare(value, this.value);
                if (compResult == 0) return true;
                if (compResult == -1)
                {
                    if (this.left == null) return false;
                    return this.left.Contains(value, comparer);
                }
                else
                {
                    if (this.right == null) return false;
                    return this.right.Contains(value, comparer);
                }
            }
        }
        #endregion
    }
}
