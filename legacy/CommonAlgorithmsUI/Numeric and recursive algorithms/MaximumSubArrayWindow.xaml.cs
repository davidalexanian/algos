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
    /// Interaction logic for MaximumSubArrayWindow.xaml
    /// </summary>
    public partial class MaximumSubArrayWindow : Window
    {
        public MaximumSubArrayWindow()
        {
            InitializeComponent();
        }

        private void solveBruteForceBtn_Click(object sender, RoutedEventArgs e)
        {
            int[] input = GetArray();
            if (input == null) return;
            var t = MaximumSubArray.GetMaxSubArrayBruteForce(input);
            resultTxtBlock.Text = string.Format("Biggest sum:{0}, start index:{1}, end index{2}",t.Item1,t.Item2,t.Item3);
            resultTxtBox.Text = "";
            for (int i = t.Item2; i <= t.Item3; i++)
                resultTxtBox.Text += input[i].ToString() + ",";
            resultTxtBox.Text = resultTxtBox.Text.Substring(0, resultTxtBox.Text.Length - 1);
        }

        private void solveFasterBtn_Click(object sender, RoutedEventArgs e)
        {
            int[] input = GetArray();
            if (input == null) return;
            var t = MaximumSubArray.GetMaxSubArray(input);
            resultTxtBlock.Text = string.Format("Biggest sum:{0}, start index:{1}, end index{2}", t.Item1, t.Item2, t.Item3);
            resultTxtBox.Text = "";
            for (int i = t.Item2; i <= t.Item3; i++)
                resultTxtBox.Text += input[i].ToString() + ",";
            resultTxtBox.Text = resultTxtBox.Text.Substring(0, resultTxtBox.Text.Length - 1);
        }
        private int[] GetArray()
        {
            try
            {
                if (string.IsNullOrEmpty(Input.Text)) throw new Exception("Input is empty");
                string[] input = Input.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int[] inputArray = new int[input.Length];
                int i = 0;
                foreach (var str in input)
                {
                    inputArray[i++] = int.Parse(str);
                }
                return inputArray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Input was not in correct format {0}",ex.Message));
                return null;
            }
        }
    }
}
