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
using CommonAlgorithms;
using CommonAlgorithms.Numerical;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for FractalTrianglesWindow.xaml
    /// </summary>
    public partial class FractalTrianglesWindow : Window
    {
        public FractalTrianglesWindow()
        {
            InitializeComponent();
        }
        private void genBtn_Click(object sender, RoutedEventArgs e)
        {
            ushort n = 0;
            if (!ushort.TryParse(depthTxtBox.Text, out n) || n > 10)
            {
                MessageBox.Show("Choose a number between 0 to 10");
                return;
            }
            var p1 = new CommonAlgorithms.Numerical.Point(300, 50);
            var p2 = new CommonAlgorithms.Numerical.Point(50, 550);
            var p3 = new CommonAlgorithms.Numerical.Point(550, 550);
            Triangle t = new Triangle(p1,p2,p3);
            List<Triangle> result = new List<Triangle>((int)System.Math.Pow(3D, (double)n));
            this.Dispatcher.Invoke(() => genBtn.IsEnabled = false, System.Windows.Threading.DispatcherPriority.Render);
            this.Dispatcher.Invoke(() => canvas.Children.Clear(), System.Windows.Threading.DispatcherPriority.Render);

            Recursive.DrawFractalTriangles(t, n, result);
            
            foreach (var triangle in result)
            {
                Polyline polyline = new Polyline();
                polyline.Points.Add(new System.Windows.Point(triangle.p1.X, triangle.p1.Y));
                polyline.Points.Add(new System.Windows.Point(triangle.p2.X, triangle.p2.Y));
                polyline.Points.Add(new System.Windows.Point(triangle.p3.X, triangle.p3.Y));
                polyline.Fill = Brushes.Black;
                polyline.Stroke = Brushes.Black;
                polyline.StrokeThickness = 1;
                canvas.Children.Add(polyline);
            }
            genBtn.IsEnabled = true;
        }
    }
}
