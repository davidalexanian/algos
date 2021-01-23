using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using CommonAlgorithms.Graphs;
using Petzold.Media2D;
using System.Threading;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for GraphWindow.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        private Graph<string, string> graph;

        private Dictionary<Node<string, string>, DrawedPointInfo> drawnNodes = new Dictionary<Node<string, string>, DrawedPointInfo>();
        private Dictionary<Link<string, string>, ArrowLineBase> drawnLinks = new Dictionary<Link<string, string>, ArrowLineBase>();
        private Brush nodeColor = Brushes.White;
        private Brush nodeStrockColor = Brushes.Black;
        private Brush nodeNameColor = Brushes.Black;
        private Brush linkColor = Brushes.Blue;
        private bool graphNodeMoving = false;
        private int nodeRadius = 15;
        private Node<string, string> currentNode = null;
        private Link<string, string> currentLink = null;
        private bool movingOccured = false;

        public GraphWindow()
        {
            InitializeComponent();
        }
        
        private void GraphGenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GraphType gt = ((ComboBoxItem)this.GraphTypeCmbBox.SelectedItem).Tag.ToString() == "DIRECTED" ? GraphType.Directed : GraphType.Undirected;
                int nodeCount = int.Parse(this.GraphGenerateNodeCountTxtBox.Text);
                int linkCount = int.Parse(this.GraphGenerateLinkCountTxtBox.Text);
                int maxLinkCount = gt == GraphType.Directed ? nodeCount * (nodeCount - 1) : (nodeCount * (nodeCount - 1)) / 2;
                if (nodeCount < 2 || linkCount < 1 || linkCount > maxLinkCount)
                {
                    MessageBox.Show("Nodes or links count is incorrect");
                    return;
                }

                this.graph = CommonAlgorithms.Utility.GenerateGraph(nodeCount, linkCount, gt);
                DrawGraph();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GraphGenerateNodeCountTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsVisible) return;
            ulong count = 0;
            if (!ulong.TryParse(this.GraphGenerateNodeCountTxtBox.Text, out count) || count < 2)
            { 
                GraphGenerateNodeCountTxtBox.Background = Brushes.Red;
                return;
            }
            GraphGenerateNodeCountTxtBox.Background = Brushes.White;
            RecalculateMaxLinkNumber();
        }

        private void RecalculateMaxLinkNumber()
        {
            ulong count = 0;
            ulong maxLinkCount = 0;
            if (!ulong.TryParse(this.GraphGenerateNodeCountTxtBox.Text, out count) || count < 2) return;
            var gt = ((ComboBoxItem)this.GraphTypeCmbBox.SelectedItem).Tag.ToString() == "DIRECTED" ? GraphType.Directed : GraphType.Undirected;
            if (gt == GraphType.Directed)
                maxLinkCount = count * (count - 1);
            else
                maxLinkCount = (count * (count - 1)) / 2;

            this.GraphInfolbl.Content = "Max number of links: " + maxLinkCount.ToString();

            // refresh link count if required
            ulong.TryParse(this.GraphGenerateLinkCountTxtBox.Text, out count);
            if (maxLinkCount < count) this.GraphGenerateLinkCountTxtBox.Text = maxLinkCount.ToString();
        }

        private void GraphTypeCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsVisible) return;
            RecalculateMaxLinkNumber();
        }
        private void GraphNode_MouseUp(object sender, MouseButtonEventArgs e)
        {
            graphNodeMoving = false;
            if (movingOccured) RedrawGraph();
            if (currentNode != null) drawnNodes[currentNode].Ellipse.Fill = Brushes.Yellow;
        }
        private void GraphNode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            movingOccured = false;
            if (currentNode != null)
            {
                drawnNodes[currentNode].Ellipse.Fill = this.nodeColor;
                currentNode = null;
            }
            if (sender is Ellipse)
            {
                currentNode = (Node<string,string>)((Ellipse)sender).Tag;
                drawnNodes[currentNode].Ellipse.Fill = Brushes.Yellow;
            }
            if (sender is Label)
            {
                currentNode = (Node<string, string>)((Label)sender).Tag;
                drawnNodes[currentNode].Ellipse.Fill = Brushes.Yellow;
            }
            graphNodeMoving = true;
            e.Handled = true;
        }
        private void GraphNode_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.graphNodeMoving) return;
            var position = e.GetPosition(this.graphCanvas);
            if (currentNode != null) movingOccured = true;

            Label l = null;
            Ellipse el = null;
            Node<string, string> node;

            if (sender is Ellipse)
            {
                el = (Ellipse)sender;
                node = (Node<string, string>)el.Tag;
                l = (Label)drawnNodes[node].Label;
            }
            else if (sender is Label)
            {
                l = (Label)sender;
                node = (Node<string, string>)l.Tag;
                el = (Ellipse)drawnNodes[node].Ellipse;
            }
            else return;

            Canvas.SetLeft(el, position.X - nodeRadius);
            Canvas.SetTop(el, position.Y - nodeRadius);

            if (l != null)
            {
                Canvas.SetLeft(l, position.X - nodeRadius + 2);
                Canvas.SetTop(l, position.Y - nodeRadius + 2);
            }
            drawnNodes[node].Point = position;
        }
        private void RedrawGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            RedrawGraph();
        }
        private void RedrawGraph()
        {
            DrawGraph(true);
        }
        private void DrawGraph(bool redraw = false)
        {
            if (graph == null) return;
            currentNode = null;
            currentLink = null;
            this.graphCanvas.Children.Clear();
            DrawNodes(redraw);
            DrawLinks();
        }
        private Point GeneratePointToDrawNode(int canvasWidth, int canvasHeight, Random r)
        {
            Point p = new Point(r.Next(nodeRadius, canvasWidth - nodeRadius), r.Next(nodeRadius, canvasHeight - nodeRadius));
            if (graph.Nodes.Count > 70) return p;
            int j = 0;

            do
            {
                bool isTooClose = false;
                foreach (var point in drawnNodes.Values.Select(v=>v.Point))
                {
                    if (GetDistance(p, point) < 4 * nodeRadius)
                    {
                        isTooClose = true;
                        break;
                    }
                }

                if (isTooClose)
                    p = new Point(r.Next(nodeRadius, canvasWidth - nodeRadius), r.Next(nodeRadius, canvasHeight - nodeRadius));
                else
                    return p;

                j++;
            }
            while (j < 25);
            return p;
        }
        private void DrawNodes(bool redraw = false)
        {
            Dictionary<Node<string, string>, Point> drawnPoints = null;
            if (redraw)
            {
                drawnPoints = new Dictionary<Node<string, string>, Point>(graph.NodesCount);
                foreach (var n in graph.Nodes)
                    if (drawnNodes.ContainsKey(n)) drawnPoints.Add(n, drawnNodes[n].Point);
            }
            drawnNodes.Clear();

            var r = new Random();
            int canvasWidth = (int)graphCanvas.ActualWidth;
            int canvasHeight = (int)graphCanvas.ActualHeight;

            for (int i = 0; i <= graph.Nodes.Count - 1; i++)
            {
                var n = graph.Nodes[i];
                Point p = new Point();
                if (redraw && drawnPoints.ContainsKey(n))
                    p = drawnPoints[n];
                else
                    p = GeneratePointToDrawNode(canvasWidth, canvasHeight, r);

                // draw ellipse
                var ellipse = new Ellipse();
                ellipse.Fill = this.nodeColor;
                ellipse.Stroke = this.nodeStrockColor;
                ellipse.StrokeThickness = 0.5;
                ellipse.Width = this.nodeRadius * 2;
                ellipse.Height = this.nodeRadius * 2;
                ellipse.MouseDown += GraphNode_MouseDown;
                ellipse.MouseMove += GraphNode_MouseMove;
                ellipse.MouseUp += GraphNode_MouseUp;
                ellipse.ToolTip = new ToolTip();
                ((ToolTip)ellipse.ToolTip).Content = "Value: " + n.Value.ToString();
                ellipse.Tag = n;

                graphCanvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, p.X - nodeRadius);
                Canvas.SetTop(ellipse, p.Y - nodeRadius);
                Canvas.SetZIndex(ellipse, 100);

                // draw label
                Label lbl = new Label();
                lbl.Content = n.Name;
                lbl.Foreground = nodeNameColor;
                lbl.MouseDown += GraphNode_MouseDown;
                lbl.MouseMove += GraphNode_MouseMove;
                lbl.MouseUp += GraphNode_MouseUp;
                lbl.Tag = n;
                lbl.FontWeight = FontWeights.Bold;

                graphCanvas.Children.Add(lbl);
                Canvas.SetLeft(lbl, p.X - nodeRadius + 2);
                Canvas.SetTop(lbl, p.Y - nodeRadius + 2);
                Canvas.SetZIndex(lbl, 101);

                drawnNodes.Add(n, new DrawedPointInfo() { Ellipse = ellipse, Label = lbl, Point = p });
            }
        }
        private void DrawLinks()
        {
            // remove all links
            drawnLinks.Clear();
            for (int i = 0; i <= graphCanvas.Children.Count - 1; i++)
            {
                var child = graphCanvas.Children[i];
                if (child is ArrowLineBase) {
                    graphCanvas.Children.RemoveAt(i);
                    i--;
                }
            }

            // draw links
            foreach (var node in drawnNodes.Keys)
            {
                Point startPosition = drawnNodes[node].Point;
                foreach (var link in node.Links)
                {
                    if (drawnLinks.ContainsKey(link)) continue;

                    if (link.IsLoop)
                    {
                        ArrowPolyline apoly = new ArrowPolyline() { Stroke = this.linkColor, StrokeThickness = 0.5, ArrowAngle = 25, ArrowLength = 6 };
                        apoly.ArrowEnds = graph.GraphType == GraphType.Undirected ? ArrowEnds.None : ArrowEnds.End;
                        Point nodePosition = drawnNodes[link.Node1].Point;
                        apoly.Points.Add(nodePosition);
                        apoly.Points.Add(new Point(nodePosition.X, nodePosition.Y + nodeRadius * 2));
                        apoly.Points.Add(new Point(nodePosition.X + nodeRadius * 2, nodePosition.Y + nodeRadius * 2));
                        apoly.Points.Add(new Point(nodePosition.X + nodeRadius * 2, nodePosition.Y));
                        apoly.Points.Add(new Point(nodePosition.X + nodeRadius, nodePosition.Y));
                        graphCanvas.Children.Add(apoly);
                        drawnLinks.Add(link, apoly);

                        Label lbl = new Label() { Content = link.Weight.ToString(), Foreground = nodeNameColor, FontWeight = FontWeights.Bold };
                        Canvas.SetLeft(lbl, apoly.Points.Last().X + nodeRadius);
                        Canvas.SetTop(lbl, apoly.Points.Last().Y + nodeRadius);
                        Canvas.SetZIndex(lbl, 101);
                        graphCanvas.Children.Add(lbl);
                    }
                    else
                    {
                        Point endPosition = drawnNodes[link.Node2].Point;
                        if (graph.GraphType == GraphType.Undirected)
                            if (node == link.Node2) endPosition = drawnNodes[link.Node1].Point;

                        Petzold.Media2D.ArrowLine line = new Petzold.Media2D.ArrowLine() { Stroke = this.linkColor, StrokeThickness = 1, ArrowAngle = 25, ArrowLength = 6};                        
                        line.ArrowEnds = graph.GraphType == GraphType.Undirected ? ArrowEnds.None : ArrowEnds.End;
                        line.X1 = startPosition.X;
                        line.Y1 = startPosition.Y;

                        Point endPoint = new Point();
                        if (endPosition.X - startPosition.X == 0 || endPosition.Y - startPosition.Y == 0)
                        {
                            endPoint.X = endPosition.X;
                            endPoint.Y = endPosition.Y;
                        }
                        else
                        {
                            double alpha = Math.Atan(Math.Abs((endPosition.Y - startPosition.Y) / (endPosition.X - startPosition.X)));
                            double sinAlpha = Math.Abs(Math.Sin(alpha));
                            double cosAlpha = Math.Abs(Math.Cos(alpha));

                            if (endPosition.X > startPosition.X && endPosition.Y < startPosition.Y)         //first quarter
                            {
                                endPoint.X = endPosition.X - ((double)nodeRadius) * cosAlpha;
                                endPoint.Y = endPosition.Y + ((double)nodeRadius) * sinAlpha;
                            }
                            else if (endPosition.X > startPosition.X && endPosition.Y > startPosition.Y)    //second quarter
                            {
                                endPoint.X = endPosition.X - ((double)nodeRadius) * cosAlpha;
                                endPoint.Y = endPosition.Y - ((double)nodeRadius) * sinAlpha;
                            }
                            else if (endPosition.X < startPosition.X && endPosition.Y > startPosition.Y)    //third quarter
                            {
                                endPoint.X = endPosition.X + ((double)nodeRadius) * cosAlpha;
                                endPoint.Y = endPosition.Y - ((double)nodeRadius) * sinAlpha;
                            }
                            else if (endPosition.X < startPosition.X && endPosition.Y < startPosition.Y)    //fourth quarter
                            {
                                endPoint.X = endPosition.X + ((double)nodeRadius) * cosAlpha;
                                endPoint.Y = endPosition.Y + ((double)nodeRadius) * sinAlpha;
                            }
                        }

                        line.X2 = endPoint.X;
                        line.Y2 = endPoint.Y;
                        graphCanvas.Children.Add(line);
                        line.MouseDown += Line_MouseDown;
                        line.Tag = link;
                        drawnLinks.Add(link, line);
                        Canvas.SetZIndex(line, 99);

                        // add link weight to canvas
                        var p = new Point((line.X1 + line.X2) / 2, (line.Y1 + line.Y2) / 2);
                        Label lbl = new Label() { Content = link.Weight.ToString(), Foreground = nodeNameColor, FontWeight = FontWeights.Bold };
                        Canvas.SetLeft(lbl,p.X - nodeRadius);
                        Canvas.SetTop(lbl, p.Y - nodeRadius);
                        Canvas.SetZIndex(lbl, 101);
                        graphCanvas.Children.Add(lbl);
                        
                    }
                }
            }
        }

        private void Line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ArrowLineBase)
            {
                if (currentLink != null)
                {
                    drawnLinks[currentLink].Stroke = this.linkColor;
                    currentLink = null;
                }
                ((ArrowLineBase)sender).Stroke = Brushes.Yellow;
                currentLink = (Link<string, string>)((Shape)sender).Tag;
            }
        }

        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private void ClearGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            this.graphCanvas.Children.Clear();
        }

        private void CanTravelTo_Click(object sender, RoutedEventArgs e)
        {
            if (currentNode == null)
            {
                MessageBox.Show("Current node is not selected");
                return;
            }
            this.GraphInfolbl.Content = "Current node:" + currentNode.Name;

            foreach (var n in drawnNodes.Keys)
                if (currentNode != n) drawnNodes[n].Ellipse.Fill = this.nodeColor;

            var nodes = graph.GetAdjacentNodesCanTravelTo(currentNode);
            foreach (var n in nodes)
                drawnNodes[n].Ellipse.Fill = Brushes.Orange;
        }

        private void CanTravelFrom_Click(object sender, RoutedEventArgs e)
        {
            if (currentNode == null)
            {
                MessageBox.Show("Current node is not selected");
                return;
            }
            this.GraphInfolbl.Content = "Current node:" + currentNode.Name;
            foreach (var n in drawnNodes.Keys)
                if (currentNode != n) drawnNodes[n].Ellipse.Fill = this.nodeColor;

            var nodes = graph.GetAdjacentNodesCanBeTravelledFrom(currentNode);
            foreach (var n in nodes)
                drawnNodes[n].Ellipse.Fill = Brushes.Orange;
        }

        private void graphCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentNode == null) return;
            drawnNodes[currentNode].Ellipse.Fill = this.nodeColor;
            currentNode = null;
        }

        private void RemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null) return;
            if (currentNode == null)
            {
                MessageBox.Show("Current node is not selected");
                return;
            }
            this.GraphInfolbl.Content = "Current node:" + currentNode.Name;

            graph.RemoveNode(currentNode);
            this.RedrawGraph();
        }
        private void RemoveLink_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null) return;
            if (currentLink == null)
            {
                MessageBox.Show("Select a link");
                return;
            }
            graph.RemoveLink(currentLink);
            this.RedrawGraph();
        }
        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null) return;
            uint start = 0;
            uint to = 0;
            if (!uint.TryParse(this.fromNodeIndex.Text, out start) || !uint.TryParse(this.toNodeIndex.Text, out to))
                MessageBox.Show("Link indexes are not in correct format");
            
            try
            {
                start--;
                to--;
                var l = new Link<string, string>(graph.Nodes[(int)start], graph.Nodes[(int)to]);
                l.LinkProperty = new Random().Next(10, 99).ToString();
                graph.AddLink(l);
                RedrawGraph();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            var name = (graph.NodesCount + 1).ToString();
            var node = new Node<string, string>();
            node.Value = name;
            node.Name = name;
            graph.AddNode(node);
            RedrawGraph();
        }
        private void Traverse_Click(object sender, RoutedEventArgs e)
        {
            if (currentNode == null) { MessageBox.Show("Choose a start node"); return; };
            ProcessNodeTraversalDelegate d = new ProcessNodeTraversalDelegate(ProcessNodeTraversal);
            StringBuilder sb = new StringBuilder();

            if (TraversalCmbBox.SelectedIndex == 0)
            {
                Graph<string, string>.DepthFirstTraversal(currentNode, n => 
                {
                    sb.Append(n.Name + " -> ");
                    Dispatcher.Invoke(d, System.Windows.Threading.DispatcherPriority.Background, n);
                    System.Threading.Thread.Sleep(600);
                });
            }
            else
            {
                Graph<string, string>.DepthFirstTraversal(currentNode, n =>
                {
                    sb.Append(n.Name + " -> ");
                    Dispatcher.Invoke(d, System.Windows.Threading.DispatcherPriority.Background, n);
                    System.Threading.Thread.Sleep(600);
                });
            }
            sb.Append("That's all");
            infoTxtBlock.Text = sb.ToString();

            foreach (var n in drawnNodes.Keys)
                drawnNodes[n].Ellipse.Fill = Brushes.White;
        }
        public void ProcessNodeTraversal(Node<string, string> n)
        {
            drawnNodes[n].Ellipse.Fill = Brushes.Red;
        }

        private void ConnectionTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(graph.IsConnected.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void minSpanTree_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null) { MessageBox.Show("Generate the graoh first"); return; }
            try
            {
                var tree = this.graph.MinSpanTree();
                double weight = 0;
                foreach (var link in tree)
                {
                    weight += link.Weight;
                    drawnLinks[link].Stroke = Brushes.Red;
                    drawnNodes[link.Node1].Ellipse.Fill = Brushes.Red;
                    drawnNodes[link.Node2].Ellipse.Fill = Brushes.Red;
                }
                infoTxtBlock.Text = string.Format("Spanning tree weight:{0}, Total weight: {1}", weight, graph.TotalLinkWeight);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void anyShortestPathFromNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (graph == null) return;
                uint start = 0;
                uint to = 0;
                if (!uint.TryParse(this.fromNodeIndex.Text, out start) || !uint.TryParse(this.toNodeIndex.Text, out to))
                {
                    MessageBox.Show("Link indexes are not in correct format");
                    return;
                }
                start--;
                to--;
                var fromNode = graph.Nodes[(int)start];
                var toNode = graph.Nodes[(int)to];
                var t = graph.AnyShortestPath(fromNode);

                StringBuilder sb = new StringBuilder();
                var tn = t.Root.FindNodeByValue(toNode);
                while (tn != null)
                {
                    sb.Append(tn.Value.Name + ">");
                    drawnNodes[tn.Value].Ellipse.Fill = Brushes.Red;
                    if (tn.Parent != null)
                    {
                        if (graph.GraphType == GraphType.Undirected)
                        {
                            var l = tn.Value.Links.Where(x => (x.Node1 == tn.Value && x.Node2 == tn.Parent.Value) || (x.Node2 == tn.Value && x.Node1 == tn.Parent.Value)).First();
                            if (l != null) drawnLinks[l].Stroke = Brushes.Red;
                        }
                        else
                        {

                            var l = tn.Parent.Value.Links.Where(x => (x.Node1 == tn.Parent.Value && x.Node2 == tn.Value)).First();
                            if (l != null) drawnLinks[l].Stroke = Brushes.Red;
                        }
                    }
                    tn = tn.Parent;
                }
                var str = sb.ToString();
                infoTxtBlock.Text = str.Substring(0, str.Length - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void topoogicalOrderingBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nodes = graph.TopoligicalOrdering();
                var sb = new StringBuilder();
                foreach (var n in nodes) sb.Append(n.Name + " > ");
                var str = sb.ToString();
                this.infoTxtBlock.Text = str.Substring(0, str.Length - 1);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    delegate void ProcessNodeTraversalDelegate(Node<string, string> n);

    public class DrawedPointInfo
    {
        public Ellipse Ellipse { get; set; }
        public Label Label { get; set; }
        public Point Point { get; set; }
    }
}
