using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CommonAlgorithms.Graphs
{
    /// <summary>
    /// Represents 
    /// </summary>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TLinkProperty"></typeparam>
    public class Graph<TNodeValue, TLinkProperty>
    {
        public Graph(GraphType type, bool allowMultigraphs = true) : this(type, 0, allowMultigraphs) { }
        public Graph(GraphType type, int capacity, bool allowMultigraphs = true)
        {
            allowMultiGraph = allowMultigraphs;
            graphType = type;
            nodes = new List<Node<TNodeValue, TLinkProperty>>(capacity);
            links = new List<Link<TNodeValue, TLinkProperty>>(capacity);
        }
        private List<Node<TNodeValue, TLinkProperty>> nodes;
        private List<Link<TNodeValue, TLinkProperty>> links;
        private GraphType graphType;
        private bool allowMultiGraph;
        public string Name { get; set; }

        public GraphType GraphType
        {
            get { return graphType; }
        }
        /// <summary>
        /// If true, graph allows adding more than one link between 2 nodes
        /// </summary>
        public bool AllowMultigraph
        {
            get { return allowMultiGraph; }
        }
        /// <summary>
        /// Count of nodes
        /// </summary>
        public int NodesCount
        {
            get { return nodes.Count; }
        }
        /// <summary>
        /// Count of links
        /// </summary>
        public int LinksCount
        {
            get { return links.Count; }
        }
        public IList<Node<TNodeValue, TLinkProperty>> Nodes
        {
            get { return nodes; }
        }
        public IList<Link<TNodeValue, TLinkProperty>> Links
        {
            get { return links; }
        }
        /// <summary>
        /// True, if undirected graph is connected (there is a path between any two node), Complexity is the same as traversal.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (this.NodesCount == 0) return true;

                if (this.graphType == GraphType.Undirected)
                {
                    int count = 0;
                    Graph<TNodeValue, TLinkProperty>.DepthFirstTraversal(this.Nodes[0], n => { count++; });
                    if (count != this.NodesCount) return false;
                    return true;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }       
        /// <summary>
        /// True if contains exactly on edge between 2 nodes
        /// </summary>
        public bool IsComplete()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// True if contains two nodes with more than one links from one node to another
        /// </summary>
        public bool IsMultiGraph()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Adds new vertex to graph (vertex must not contains any links,otherwise use ConcatGraph method). Complexity is O(1)
        /// </summary>
        public void AddNode(Node<TNodeValue, TLinkProperty> node)
        {
            if (node.Graph != null) throw new InvalidOperationException("Node must not have a graph associated with it");
            if (node == null) throw new ArgumentNullException("node");
            if (node.Links.Count != 0) throw new InvalidOperationException("Node must not contain links to other nodes");
            node.Graph = this;
            this.nodes.Add(node);
        }
        /// <summary>
        /// Removes vertex and all links associated with it. Worst case complexity is quadratic (graph stores all links in seperate collection)
        /// </summary>
        public void RemoveNode(Node<TNodeValue, TLinkProperty> node)
        {
            if (node == null) throw new ArgumentNullException();
            if (node.Graph != this) throw new InvalidOperationException("Node must belong to this graph");

            for (int i = links.Count - 1; i >= 0; i--)
                if (links[i].Node1 == node || links[i].Node2 == node) this.RemoveLink(links[i]);
            this.Nodes.Remove(node);
        }
        /// <summary>
        /// Adds new link to the graph. Complexity is O(n^2).
        /// </summary>
        public void AddLink(Link<TNodeValue, TLinkProperty> link)
        {
            if (link == null) throw new ArgumentException("link");
            if (link.Node1 == null || link.Node2 == null) throw new ArgumentException("Nodes of link are not initialized");
            if (link.Node1.Graph != this || link.Node2.Graph != this) throw new ArgumentException("Both nodes of link must belong to the graph"); 

            if (!allowMultiGraph)
            {
                foreach (var l in link.Node1.Links)
                    if (l.Node1 == link.Node1 && l.Node2 == link.Node2)
                        throw new InvalidOperationException("Such a link already exists and multigraphs are not allowed");
            }

            link.Node1.Links.Add(link);
            this.links.Add(link);
            if (this.graphType == GraphType.Undirected) link.Node2.Links.Add(link);
        }
        /// <summary>
        /// Adds new link to the graph. Worst case complexity is quadratic (if the graph is complete).
        /// </summary>
        public void AddLink(Node<TNodeValue, TLinkProperty> fromNode, Node<TNodeValue, TLinkProperty> toNode)
        {
            if (fromNode == null || toNode == null) throw new ArgumentException("Nodes are specified incorrectly");
            if (fromNode.Graph != toNode.Graph || fromNode.Graph != this) throw new InvalidOperationException("Nodes must belong to this graph");

            if (!allowMultiGraph)
            {
                foreach (var l in fromNode.Links)
                    if (l.Node1 == fromNode && l.Node2 == toNode)
                        throw new InvalidOperationException("Such a link already exists and multigraphs are not allowed");
            }
            AddLink(new Link<TNodeValue, TLinkProperty>(fromNode, toNode));
        }
        /// <summary>
        /// Returns a collection of nodes adjacent to specifed node so that tey can be travelled from given node.
        /// </summary>
        public IEnumerable<Node<TNodeValue, TLinkProperty>> GetAdjacentNodesCanTravelTo(Node<TNodeValue,TLinkProperty> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (node.Graph != this) throw new Exception("Node must belong to this graph");
            var nodes = new Dictionary<Node<TNodeValue, TLinkProperty>,bool>();

            foreach (var link in node.Links)
            {
                if (node == link.Node1 && !nodes.ContainsKey(link.Node2)) nodes.Add(link.Node2, true);
                if (this.graphType == GraphType.Undirected && node == link.Node2 && !nodes.ContainsKey(link.Node1)) nodes.Add(link.Node1, true);
            }
            return nodes.Keys;
        }
        /// <summary>
        /// Returns a collection of nodes adjacent to specified node so that given node can be rached from those nodes.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<Node<TNodeValue, TLinkProperty>> GetAdjacentNodesCanBeTravelledFrom(Node<TNodeValue, TLinkProperty> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (node.Graph != this) throw new Exception("Node must belong to this graph");
            var nodes = new Dictionary<Node<TNodeValue, TLinkProperty>, bool>();
            var g = node.Graph;

            foreach (var link in g.Links)
            {
                if (g.graphType == GraphType.Directed)
                {
                    if (link.Node2 == node && !nodes.ContainsKey(link.Node1)) nodes.Add(link.Node1, true);
                }
                else
                {
                    if (link.Node2 == node && !nodes.ContainsKey(link.Node1)) nodes.Add(link.Node1, true);
                    if (link.Node1 == node && !nodes.ContainsKey(link.Node2)) nodes.Add(link.Node2, true);
                }
            }
            return nodes.Keys;
        }
        /// <summary>
        /// Removes first occurance of the link between specifie nodes. Note that nodes might still remain connected (e.g. multigraphs or directed graphs). Complexity is quadratic. 
        /// Use disconnect to remove all the connecting links.
        /// </summary>
        public bool RemoveLink(Node<TNodeValue, TLinkProperty> fromNode, Node<TNodeValue, TLinkProperty> toNode)
        {
            if (fromNode == null || toNode == null) throw new ArgumentNullException("Nodes are specified incorrectly");
            if (fromNode.Graph != toNode.Graph || fromNode.Graph != this) throw new InvalidOperationException("Nodes must belong to this graph");

            Link<TNodeValue, TLinkProperty> link = null;
            foreach (var l in links)
                if (l.Node1 == fromNode && l.Node2 == toNode)
                {
                    link = l;
                    break;
                }

            if (link != null)
            {
                RemoveLink(link);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes given link from the graph. Worst case complexity is quadratic.
        /// </summary>
        public void RemoveLink(Link<TNodeValue, TLinkProperty> link)
        {
            if (link == null) throw new ArgumentNullException("link");
            if (link.Node1 == null || link.Node2 == null) throw new InvalidOperationException("Link is invalid");
            if (link.Node1.Graph != link.Node2.Graph || link.Node2.Graph != this) throw new InvalidOperationException("Link is invalid");
            this.links.Remove(link);
            link.Node1.Links.RemoveAll(x=> x == link);
            link.Node2.Links.RemoveAll(x => x == link);
        }        
        /// <summary>
        /// Removes nodes and links
        /// </summary>
        public void Clear()
        {
            this.Nodes.Clear();
            this.Links.Clear();
        }
        public override string ToString()
        {
            return this.Name;
        }
        /// <summary>
        /// Returns total weight of all nodes
        /// </summary>
        public double TotalNodeWeight
        {
            get {
                double w = 0;
                foreach (var n in Nodes) w += n.Weight;
                return w; 
            }
        }
        /// <summary>
        /// Returns total weight of all links
        /// </summary>
        public double TotalLinkWeight
        {
            get {
                double w = 0;
                foreach (var l in Links) w += l.Weight;
                return w;
            }
        }

        #region "Spanning Tree and Paths"
        /// <summary>
        /// Returns minimum spanning tree using a greedy algorithm (similar to Prim's algorithm but not exactly the same). Complexity is O(n^2), where n is the number of nodes.
        /// </summary>
        /// <remarks>Unlike Dijkstra's algorithm, this one sets the labels of nodes only once</remarks>
        public IEnumerable<Link<TNodeValue, TLinkProperty>> MinSpanTree()
        {
            if (NodesCount == 0) return new List<Link<TNodeValue, TLinkProperty>>();
            var treeNodes = new Dictionary<Node<TNodeValue, TLinkProperty>, bool>();
            var treeLinks = new Dictionary<Link<TNodeValue, TLinkProperty>, bool>();

            // add any node (as min span tree will include all the nodes there is no difference where we start)
            treeNodes.Add(Nodes[0], false);

            // make a list of candidate links where we will search a link with min. weight such that connects a node from tree with a node outside the tree
            var candidateLinks = new Dictionary<Link<TNodeValue, TLinkProperty>, bool>();
            foreach (var l in Nodes[0].Links) if (! l.IsLoop) candidateLinks.Add(l, false);

            while (true)
            {
                if (treeNodes.Count == this.NodesCount) break;  // if all nodes are in treeNodes, exit

                // select a link with least cost among all candidate links that connects a node from tree with a node outside the tree
                Link<TNodeValue, TLinkProperty> leastLink = null;
                foreach (var l in candidateLinks.Keys)
                {
                    if (candidateLinks[l] == false)
                    {
                        if (leastLink == null) leastLink = l;
                        else if (l.Weight < leastLink.Weight) leastLink = l;
                    }
                }
                if (leastLink == null) break;
                candidateLinks[leastLink] = true;   // mark as used

                // add link’s destination node to the spanning tree
                Node<TNodeValue, TLinkProperty> nodeToAdd = null;
                if (!treeNodes.ContainsKey(leastLink.Node1))
                    nodeToAdd = leastLink.Node1;
                else if (!treeNodes.ContainsKey(leastLink.Node2))
                    nodeToAdd = leastLink.Node2;

                if (nodeToAdd == null)
                    candidateLinks[leastLink] = true;   // mark as used to exclude further usage as the destination node is already added
                else
                {
                    treeNodes.Add(nodeToAdd, false);    // add node to tree node
                    treeLinks.Add(leastLink, false);    // add selected link to the spanning tree
                    foreach (var l in nodeToAdd.Links) if (!l.IsLoop && !candidateLinks.ContainsKey(l)) candidateLinks.Add(l, false);
                }
            }

            if (treeNodes.Count != NodesCount)
                if (GraphType == GraphType.Undirected) throw new Exception("Graph is not connected");
                else throw new Exception("Not possible to follow all nodes");

            return treeLinks.Keys;
        }        
        /// <summary>
        /// Returns a spanning tree rooted at given node to find a shortest path from any node to the root node.
        /// </summary>
        public DataStructures.Tree<Node<TNodeValue,TLinkProperty>> AnyShortestPath(Node<TNodeValue, TLinkProperty> fromNode)
        {
            if (fromNode == null || fromNode.Graph != this) throw new Exception("Arguments are incorrect");
            var treeNodes = new Dictionary<Node<TNodeValue, TLinkProperty>, bool>();
            var treeLinks = new Dictionary<Link<TNodeValue, TLinkProperty>, bool>();
            var tree = new DataStructures.Tree<Node<TNodeValue, TLinkProperty>>(fromNode);      //resulting spanning tree
            if (NodesCount == 0) return tree;
            treeNodes.Add(fromNode, false);
            var candidateLinks = new Dictionary<Link<TNodeValue, TLinkProperty>,bool>();
            foreach (var l in fromNode.Links) if (!l.IsLoop) candidateLinks.Add(l,false);             // add all links of fromNode to list
            foreach (var n in nodes) n.Weight = 0;                                              // reset weights of all nodes

            while (true)
            {
                if (candidateLinks.Count == 0 || treeNodes.Count == NodesCount) break;     // all nodes are processed

                // find a node not in the tree that has the smallest distance from starting node
                double smallestWeight = 0;
                Link<TNodeValue, TLinkProperty> closestLink = null;
                Node<TNodeValue, TLinkProperty> closestNode = null;

                foreach (var l in candidateLinks.Keys)
                {
                    if (!candidateLinks[l])
                    {
                        Node<TNodeValue, TLinkProperty> nodeInTree, nodeOutTree = null;
                        if (treeNodes.ContainsKey(l.Node1))
                        {
                            nodeInTree = l.Node1;
                            nodeOutTree = l.Node2;
                        }
                        else
                        {
                            nodeInTree = l.Node2;
                            nodeOutTree = l.Node1;
                        }

                        double newWeight = nodeInTree.Weight + l.Weight;
                        if (closestLink == null || newWeight < smallestWeight)
                        {
                            closestLink = l;
                            closestNode = nodeOutTree;
                            smallestWeight = newWeight;
                        }
                    }
                }

                // if both nodes of the link are in the tree, mark the link as added
                if (closestNode == null) break;
                if (treeNodes.ContainsKey(closestNode))
                {
                    candidateLinks[closestLink] = true;
                }
                else
                {
                    // add found node to the spanning tree, set its weight, add node's links that lead to a node not yet in the tree, to the candidate links
                    closestNode.Weight = smallestWeight;
                    treeNodes.Add(closestNode, false);
                    foreach (var l in closestNode.Links)
                    {
                        var nodeNotInTree = l.Node1 == closestNode ? l.Node2 : l.Node1;
                        if (!treeNodes.ContainsKey(nodeNotInTree) && !l.IsLoop && !candidateLinks.ContainsKey(l)) candidateLinks.Add(l, false);
                    }
                    var parent = tree.Root.FindNodeByValue(closestLink.Node1 == closestNode ? closestLink.Node2 : closestLink.Node1);
                    parent.AddChild(closestNode);
                }
            }

            // check if all nodes have been added (the graph might not be connected)
            if (treeNodes.Count != this.NodesCount)
            {
                if (GraphType == GraphType.Undirected) throw new Exception("Graph is not connected");
                else throw new Exception("Not possible to follow all nodes");
            }

            return tree;
        }
        /// <summary>
        /// Returns true if the graph contains cycles
        /// </summary>
        public bool ContainsCycle()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Returns a topological ordering of the directed graph (non-empty acyclic graph has at least one such ordering).
        /// </summary>
        public List<Node<TNodeValue, TLinkProperty>> TopoligicalOrdering()
        {
            if (this.GraphType != GraphType.Directed) throw new Exception("Graph is not directed");

            // find nodes without dependent nodes (number before = null)
            Dictionary<Node<TNodeValue, TLinkProperty>, int> numberBefore = new Dictionary<Node<TNodeValue, TLinkProperty>, int>();
            var ordering = new List<Node<TNodeValue, TLinkProperty>>(this.NodesCount);
            foreach (var n in this.nodes) numberBefore.Add(n, 0);
            foreach (var l in this.links) numberBefore[l.Node2] += 1;
            if (numberBefore.Count != this.NodesCount) throw new Exception("The graph is not connected");

            while (true)
            {
                if (numberBefore.Count == 0) break;

                // find a node which does not have a dependency
                Node<TNodeValue, TLinkProperty> node = null;
                foreach (var n in numberBefore.Keys) if (numberBefore[n] == 0) { node = n; break; }

                if (node == null) throw new Exception("The graph has a circular dependency");
                numberBefore.Remove(node);
                ordering.Add(node);
                foreach (var l in node.Links) numberBefore[l.Node2] -= 1;   // decreament the amount of dependant nodes
            }

            return ordering;
        }
        #endregion

        #region "Traversal"        
        /// <summary>
        /// Breadth-First Traversal (traverse nodes closer to the start node before the nodes that are farther). Complexity is linear.
        /// </summary>
        /// <param name="startingNode"></param>
        /// <param name="processNodeAction">Action delegate which is invoked for each node</param>
        public static void BreadthFirstTraversal(Node<TNodeValue, TLinkProperty> startingNode, Action<Node<TNodeValue, TLinkProperty>> processNodeAction)
        {
            if (startingNode == null) throw new ArgumentNullException("startingNode");
            if (startingNode.Graph == null) throw new Exception("Node is not associated with a graph");
            var mountedNodes = new Dictionary<Node<TNodeValue, TLinkProperty>, bool>(startingNode.Graph.NodesCount);

            var queue = new DataStructures.Queue<Node<TNodeValue, TLinkProperty>>();
            queue.Enqueue(startingNode);
            mountedNodes.Add(startingNode, true);

            while (queue.Count != 0)
            {
                var n = queue.Dequeue();
                if (processNodeAction != null) processNodeAction(n);

                foreach (var l in n.Links)
                {
                    if (n == l.Node1 && !mountedNodes.ContainsKey(l.Node2)) 
                    {
                        queue.Enqueue(l.Node2);
                        mountedNodes.Add(l.Node2, true);
                    }
                    if (n == l.Node2 && !!mountedNodes.ContainsKey(l.Node1))
                    {
                        queue.Enqueue(l.Node1);
                        mountedNodes.Add(l.Node1,true);
                    }
                }
            }
        }
        /// <summary>
        /// Depth-First traversal (traverse nodes farther from the start node before the nodes that are closer). Complexity is linear.
        /// </summary>
        /// <param name="startingNode">Node to start traversal from</param>
        /// <param name="processNodeAction">Action delegate executed for each node</param>
        public static void DepthFirstTraversal(Node<TNodeValue, TLinkProperty> startingNode, Action<Node<TNodeValue, TLinkProperty>> processNodeAction)
        {
            if (startingNode == null) throw new ArgumentNullException("startingNode");
            if (startingNode.Graph == null) throw new Exception("Node is not associated with a graph");
            var mountedNodes = new Dictionary<Node<TNodeValue, TLinkProperty>, bool>(startingNode.Graph.NodesCount);

            var stack = new DataStructures.Stack<Node<TNodeValue, TLinkProperty>>();
            stack.Push(startingNode);
            mountedNodes.Add(startingNode, true);

            while (stack.Count != 0)
            {
                var n = stack.Pop();
                if (processNodeAction != null) processNodeAction(n);

                foreach (var l in n.Links)
                {
                    if (n == l.Node1 && !mountedNodes.ContainsKey(l.Node2))
                    {
                        stack.Push(l.Node2);
                        mountedNodes.Add(l.Node2, true);
                    }
                    if (n == l.Node2 && !mountedNodes.ContainsKey(l.Node1))
                    {
                        stack.Push(l.Node1);
                        mountedNodes.Add(l.Node1, true);
                    }
                }
            }
        }
        #endregion
    }
    public enum GraphType
    {
        Undirected, Directed
    }
}
