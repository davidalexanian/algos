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
using CommonAlgorithms.SortSearch;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for SortAndSearch.xaml
    /// </summary>
    public partial class SortAndSearchWindow : Window
    {
        public SortAndSearchWindow()
        {
            InitializeComponent();
        }
        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchValue = int.Parse(this.searchtxtBox.Text);
                var str = resulttxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int[] values = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                    values[i] = int.Parse(str[i]);

                int index = SortSearch<int>.LinearSearch(values,searchValue);
                searchResult.Text = index == -1 ? "not found" : index.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = int.Parse(NTxtBox.Text);
                var r = new CommonAlgorithms.Numerical.CongruentalRandomNumberGenerator();
                if (fromRange.IsChecked.HasValue && fromRange.IsChecked.Value)
                {
                    int minValue = int.Parse(fromTxtBox.Text);
                    int maxValue = int.Parse(toTxtBox.Text);
                    int[] seq = r.GenerateSequence(n, minValue, maxValue);
                    resulttxtBox.Text = seq.ToSimbolSeperatedString();
                }
                else
                {
                    uint[] seq = r.GenerateSequence(n);
                    resulttxtBox.Text = seq.ToSimbolSeperatedString();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void sortBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(resulttxtBox.Text)) return;
            try
            {
                var str = resulttxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int[] values = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    values[i] = int.Parse(str[i]);
                }

                if (insSort.IsChecked.HasValue && insSort.IsChecked.Value) SortSearch<int>.InsertionSort(values);
                if (selSort.IsChecked.HasValue && selSort.IsChecked.Value) SortSearch<int>.SelectionSort(values);
                if (bubSort.IsChecked.HasValue && bubSort.IsChecked.Value) SortSearch<int>.BubbleSort(values);
                if (heapSort.IsChecked.HasValue && heapSort.IsChecked.Value) SortSearch<int>.HeapSort(values);
                if (mergeSort.IsChecked.HasValue && mergeSort.IsChecked.Value) SortSearch<int>.MergeSort(values);
                if (quickSort.IsChecked.HasValue && quickSort.IsChecked.Value) SortSearch<int>.QuickSort(values);
                if (countSort.IsChecked.HasValue && countSort.IsChecked.Value) SortSearch<int>.CountingSort((uint[])(object)values);   // yes, it is wrong way to go :-)
                if (bucketSort.IsChecked.HasValue && bucketSort.IsChecked.Value) MessageBox.Show("Not implemented yet");

                resulttxtBox.Text = values.ToSimbolSeperatedString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rndBtn_Click(object sender, RoutedEventArgs e)
        {
            var input = resulttxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            CommonAlgorithms.Numerical.CongruentalRandomNumberGenerator.RandomizeInput<string>(input);
            resulttxtBox.Text = input.ToSimbolSeperatedString();
        }

        private void searchBinbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchValue = int.Parse(this.searchtxtBox.Text);
                var str = resulttxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int[] values = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                    values[i] = int.Parse(str[i]);

                int index = SortSearch<int>.BinarySearch((IList<int>)values, searchValue);
                searchResult.Text = index == -1 ? "not found" : index.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
