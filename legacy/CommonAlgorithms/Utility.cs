using System;
using System.Collections.Generic;
using System.Text;
using CommonAlgorithms.Graphs;

namespace CommonAlgorithms
{
    static public class Utility
    {
        /// <summary>
        /// Returns a number from given range corresponding to a value(any value, from range or not) using the formula result=(max-min) * value / 10 ^ simbols_count_of_value
        /// </summary>
        public static int FitNumberIntoRange(int min, int max, uint value)
        {
            if (min > max) throw new ArgumentException("min value must be less than max value");
            int charsCount = value.ToString().Length;
            double d1 = (double)value;
            double d2 = Math.Pow(10, charsCount);
            double d3 = (d1 / d2) * (max - min);
            return min + (int)d3;
        }
        public static string ToSimbolSeperatedString(this System.Collections.IEnumerable numbers, string simbol = ",")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in numbers)
                sb.Append(item + simbol);

            var str = sb.ToString();
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Substring(0, str.Length - simbol.Length);
        }        
        public static string SetOfSetsToString(this List<List<string>> l)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var set in l)
            {
                sb.Append("(");
                sb.Append(ToSimbolSeperatedString(set));
                sb.Append("), ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
        public static string ToString<T>(this CommonAlgorithms.DataStructures.Tree<T> tree)
        {
            StringBuilder sb = new StringBuilder();
            TreeToString(tree.Root, sb);
            return sb.ToString();
        }
        public static void TreeToString<T>(CommonAlgorithms.DataStructures.TreeNode<T> node, StringBuilder sb)
        {
            if (node != null)
            {
                sb.Append(node.Value.ToString());
                sb.Append(Environment.NewLine);
                foreach (var item in node.Children)
                    TreeToString(node, sb);
            }
            foreach (var item in node.Children)
                TreeToString(node, sb);
        }
        public static Graph<string, string> GenerateGraph(int nodeCount, long linkCount, GraphType gt)
        {
            var dict = new Dictionary<string, bool>();
            Random r = new Random();

            // nodes
            var graph = new Graph<string, string>(gt, false);
            for (int i = 0; i <= nodeCount-1; i++)
                graph.AddNode(new Node<string, string>() { Value = (i+1).ToString(), Name = (i+1).ToString() });

            // links
            int generatedCount = 0;
            int iterationsCount = 0;
            int maxIterationsCount = (int) Math.Max(linkCount * linkCount, 20);
            while (true)
            {
                iterationsCount++;
                int firstNode = r.Next(0, nodeCount-1);
                int secondNode = r.Next(0, nodeCount);
                if (secondNode > nodeCount-1) secondNode = nodeCount-1;
                if (firstNode != secondNode)
                {
                    string key = firstNode.ToString() + secondNode.ToString();
                    if (!dict.ContainsKey(key))
                    {
                        var newLink = new Link<string, string>(graph.Nodes[firstNode], graph.Nodes[secondNode]);
                        newLink.Weight = r.Next(10, 99);
                        graph.AddLink(newLink);
                        dict.Add(key, true);
                        key = secondNode.ToString() + firstNode.ToString();
                        if (gt == GraphType.Undirected && !dict.ContainsKey(key)) dict.Add(key, true);
                        generatedCount++;
                    }
                }
                if (generatedCount == linkCount) break;
                if (iterationsCount > maxIterationsCount) break;
            }

            return graph;
        }
    }
}
