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
    /// Interaction logic for EightQueenWindow.xaml
    /// </summary>
    public partial class EightQueenWindow : Window
    {
        private ushort currentColumn = 0;
        private ushort currentRow = 0;

        public EightQueenWindow()
        {
            InitializeComponent();
            DrawScene();
        }
        private void DrawScene()
        {
            boardGrid.Children.Clear();
            Rectangle r;
            for (int i=0; i<8; i++)
                for (int j=0; j<8; j++)
                {
                    r = new Rectangle();
                    r.Width = 50;
                    r.Height = 50;
                    r.Stroke = Brushes.Black;
                    r.StrokeThickness = 1;

                    if (i%2==1)
                        r.Fill = (j % 2 == 1 ? Brushes.Brown : Brushes.DarkOrange);
                    else
                        r.Fill = (j % 2 == 1 ? Brushes.DarkOrange: Brushes.Brown);

                    boardGrid.Children.Add(r);
                    Grid.SetRow(r, i);
                    Grid.SetColumn(r, j);
                }
        }
        private void Grid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Rectangle)) return;

            //remove old border if exists
            foreach (var item in boardGrid.Children)
            {
                Rectangle r = item as Rectangle;
                if (r != null && r.Stroke != Brushes.Black)
                {
                    r.Stroke = Brushes.Black;
                    r.StrokeThickness = 1;
                }
            }
            Rectangle rect = (Rectangle)e.Source;
            rect.Stroke = Brushes.Yellow;
            rect.StrokeThickness = 2;
            currentColumn = (ushort)Grid.GetColumn(rect);
            currentRow = (ushort)Grid.GetRow(rect);
        }
        private void startOverBtn_Click(object sender, RoutedEventArgs e)
        {
            DrawScene();
            solveBtn.IsEnabled = true;
        }
        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            var problem = new EightQueensProblem(this.currentRow, this.currentColumn);
            if (problem.Solve() == false)
            {
                MessageBox.Show("Can't solve the problem");
                return;
            }
            // draw queens on window
            bool drawQueen = false;
            TextBlock t;
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    drawQueen = problem.Positions[row, col].HasValue && problem.Positions[row, col].Value;
                    if (drawQueen)
                    {
                        t = new TextBlock();
                        t.VerticalAlignment = VerticalAlignment.Center;
                        t.HorizontalAlignment = HorizontalAlignment.Center;
                        t.Text = "Q";
                        t.FontSize = 25;
                        t.Foreground = Brushes.White;
                        boardGrid.Children.Add(t);
                        Grid.SetRow(t, row);
                        Grid.SetColumn(t, col);
                    }
                }

            solveBtn.IsEnabled = false;
        }
    }
}
