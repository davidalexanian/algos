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
using CommonAlgorithms.Numerical;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for KochCurvesWindow.xaml
    /// </summary>
    public partial class KochCurvesWindow : Window
    {
        public KochCurvesWindow()
        {
            InitializeComponent();
        }

        private void depthBtn_Click(object sender, RoutedEventArgs e)
        {
            ushort depth = 0;
            if (!ushort.TryParse(depthTxtBox.Text, out depth) || depth > 10)
            {
                MessageBox.Show("Select depth between 0 and 10");
                return;
            }
            depthBtn.IsEnabled = false;
            canvas.Children.Clear();
            List<CommonAlgorithms.Numerical.Line> result = new List<CommonAlgorithms.Numerical.Line>();
            Recursive.DrawKochCurve(depth, new CommonAlgorithms.Numerical.Point(30, 50), 0, 500, result);
            foreach (var line in result)
            {
                var l = new System.Windows.Shapes.Line();
                l.X1 = line.p1.X;
                l.Y1 = line.p1.Y;
                l.X2 = line.p2.X;
                l.Y2 = line.p2.Y;
                l.Stroke = Brushes.Red;
                l.StrokeThickness = 1;
                canvas.Children.Add(l);
            }
            depthBtn.IsEnabled = true;
        }
    }
}
