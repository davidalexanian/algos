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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void numericalAlgorithmsWindows_click(object sender, RoutedEventArgs e)
        {
            new RandomNumbersWindow().Show();
        }
        private void hanoiTowerBtn_Click(object sender, RoutedEventArgs e)
        {
            new HanoiTowerWindow().Show();
        }
        private void KochCurvesBtn_Click(object sender, RoutedEventArgs e)
        {
            new KochCurvesWindow().Show();
        }
        private void FractalTrianglesCurvesBtn_Click(object sender, RoutedEventArgs e)
        {
            new FractalTrianglesWindow().Show();
        }
        private void EightQueenBtn_Click(object sender, RoutedEventArgs e)
        {
            new EightQueenWindow().Show();
        }
        private void SelectionPermutation_Click(object sender, RoutedEventArgs e)
        {
            new CombinationsAndPermutationsWindow().Show();
        }
        private void numericalAlgorithms_Click(object sender, RoutedEventArgs e)
        {
            new NumericAlgorithmsWindow().Show();
        }
        private void SortSearch_Click(object sender, RoutedEventArgs e)
        {
            new SortAndSearchWindow().Show();
        }
        private void String_Click(object sender, RoutedEventArgs e)
        {
            new MixAlgorithmsWindow().Show();
        }

        private void BasicGraphs_Click(object sender, RoutedEventArgs e)
        {
            new GraphWindow().Show();
        }

        private void DesicionTrees_Click(object sender, RoutedEventArgs e)
        {
            new TicTacToeWindow().Show();
        }
    }
}
