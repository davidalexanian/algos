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
    /// Interaction logic for HanoiTowerWindow.xaml
    /// </summary>
    public partial class HanoiTowerWindow : Window
    {
        private HanoiTower tower;
        public HanoiTowerWindow()
        {
            InitializeComponent();
            this.diskNumbersTxt.Text = "5";
            tower = new HanoiTower(5);
            DrawTower();
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            ushort count=0;
            ushort speed;
            if (!ushort.TryParse(diskNumbersTxt.Text, out count) || (count < 1 || count > 10))
            {
                MessageBox.Show("Choose a number between 1 and 10");
                return;
            }
            if (speedTxt.Text.Trim() != string.Empty)
                if (!ushort.TryParse(speedTxt.Text, out speed) || (speed < 1 || speed > 5)) {
                    MessageBox.Show("Choose speed from 1 to 5");
                    return;
                }
            tower = new HanoiTower(count);
            start.IsEnabled = false;
            tower.DiskMoved += Tower_DiskMoved;
            tower.Solve(1, 3, count);
            start.IsEnabled = true;
        }
        private void Tower_DiskMoved(ushort diskNumber)
        {
            this.Dispatcher.Invoke(DrawTower, System.Windows.Threading.DispatcherPriority.Render);
            if (speedTxt.Text.Trim() != string.Empty) System.Threading.Thread.Sleep(ushort.Parse(speedTxt.Text) * 100);
        }
        private void DrawTower()
        {
            canvas.Children.Clear();
            // draw scene & pegs
            Rectangle scene = new Rectangle() {Width = 500, Height=20, Fill=Brushes.LightGreen, Stroke=Brushes.Black, StrokeThickness=0.5};
            Canvas.SetBottom(scene, 0);
            Rectangle peg1 = new Rectangle() { Width=3, Height=150,Stroke=Brushes.Black, StrokeThickness=0.5, Fill=Brushes.LightBlue };
            Canvas.SetBottom(peg1, 20);
            Canvas.SetLeft(peg1, 103);
            Rectangle peg2 = new Rectangle() { Width = 3, Height = 150, Stroke = Brushes.Black, StrokeThickness = 0.5, Fill = Brushes.LightBlue };
            Canvas.SetBottom(peg2, 20);
            Canvas.SetLeft(peg2, 243);
            Rectangle peg3 = new Rectangle() { Width = 3, Height = 150, Stroke = Brushes.Black, StrokeThickness = 0.5, Fill = Brushes.LightBlue };
            Canvas.SetBottom(peg3, 20);
            Canvas.SetLeft(peg3, 394);
            canvas.Children.Add(scene);
            canvas.Children.Add(peg1);
            canvas.Children.Add(peg2);
            canvas.Children.Add(peg3);

            Rectangle rec;
            for (int pegNumber=0; pegNumber < 3; pegNumber++)
            {
                var peg = tower.pegs[pegNumber];
                for(int i= peg.Count - 1; i >=0; i--)
                {
                    var disk = peg.ElementAt(i);        //reverse direction (bottom-top)
                    rec = new Rectangle() { Height = 10, Fill = Brushes.Coral, Stroke = Brushes.Black, StrokeThickness = 0.5 };
                    rec.Width = disk.DiskNumber * 100 / tower.DisksCount;
                    canvas.Children.Add(rec);

                    double d = 50;
                    d = d / tower.DisksCount;
                    d = d * (peg.Count - 1 - i);
                    if (pegNumber == 0) Canvas.SetLeft(rec, 103-rec.Width/2); //55
                    if (pegNumber == 1) Canvas.SetLeft(rec, 244-rec.Width/2); //195
                    if (pegNumber == 2) Canvas.SetLeft(rec, 395-rec.Width/2); //345
                    Canvas.SetBottom(rec, 20 + (peg.Count-1-i) * 10);
                }
            }
        }
    }
}
