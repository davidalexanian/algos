using System;
using System.Linq;
using System.Collections.Generic;

namespace CommonAlgorithms.Graphs
{
    /// <summary>
    /// Represents an edge. For directed graphs Node1 is counted as starting and Node2 as teminating node.
    /// </summary>
    /// <typeparam name="TNodeValue">Type of node's value</typeparam>
    /// <typeparam name="TLinkProperty">Type of link's property</typeparam>
    public class Link<TNodeValue, TLinkProperty>
    {
        public Link(Node<TNodeValue, TLinkProperty> n1, Node<TNodeValue, TLinkProperty> n2)
        {
            if (n1 == null) throw new ArgumentNullException("node1");
            if (n2 == null) throw new ArgumentNullException("node2");
            Node1 = n1; Node2 = n2;
        }

        public Node<TNodeValue, TLinkProperty> Node1 { get; private set; }
        public Node<TNodeValue, TLinkProperty> Node2 { get; private set; }
        public TLinkProperty LinkProperty;
        public double Weight;
        public bool IsLoop
        {
            get { return Node1 == Node2; }
        }
        public override string ToString()
        {
            return this.Node1.ToString() + "-" + this.Node2.ToString();
        }
    }
    /// <summary>
    /// Represents graph node
    /// </summary>
    public class Node<TNodeValue, TLinkProperty>
    {
        public string Name { get; set; }
        public TNodeValue Value { get; set; }
        public double Weight;
        private List<Link<TNodeValue, TLinkProperty>> links = new List<Link<TNodeValue, TLinkProperty>>();
        public Graph<TNodeValue, TLinkProperty> Graph { get; internal set; }

        public List<Link<TNodeValue, TLinkProperty>> Links
        {
            get { return links; }
        }
        /// <summary>
        /// Degree of the node for undirected graphs (loops count twice)
        /// </summary>
        public int Degree
        {
            get
            {
                int degree = 0;
                foreach (var l in this.links)
                {
                    degree += 1;
                    if (l.IsLoop) degree += 1;
                }
                return degree;
            }
        }
        /// <summary>
        /// Number of links having this node as starting node for directed graphs (loop counts once)
        /// </summary>
        public int OutDegree
        {
            get { return links.Where(l => l.Node1 == this).Count(); }
        }
        /// <summary>
        /// Number of links having this node as terminating node for directed graphs (loop counts once)
        /// </summary>
        public int InDegree
        {
            get { return links.Where(l => l.Node2 == this).Count(); }
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
