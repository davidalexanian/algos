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
    /// Interaction logic for CombinationsAndPermutationsWindow.xaml
    /// </summary>
    public partial class CombinationsAndPermutationsWindow : Window
    {
        public CombinationsAndPermutationsWindow()
        {
            InitializeComponent();
        }
        private void genNCombinationsBtn_Click(object sender, RoutedEventArgs e)
        {
            
            uint n = 0;
            if (!uint.TryParse(NtxtBox.Text, out n))
            {
                MessageBox.Show("Choose a positive integer N not exceeding cardinality of the set");
                return;
            }
            var set = setTxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            try
            { 
                var result = CombinationsAndPermutations<string>.NCombinations(set, n);
                resultTxtBox.Text = result.SetOfSetsToString();
                resultTextArea.Text = "Result (" + result.Count.ToString() + " combinations)";
            }
            catch (System.Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void genNPermutationsBtn_Click(object sender, RoutedEventArgs e)
        {
            uint n = 0;
            if (!uint.TryParse(NtxtBox.Text, out n))
            {
                MessageBox.Show("Choose a positive integer N not exceeding cardinality of the set");
                return;
            }
            var set = setTxtBox.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            try
            {
                var result = CombinationsAndPermutations<string>.NPermutations(set, n, (duplicatesAllowedCheckBox.IsChecked.HasValue ? duplicatesAllowedCheckBox.IsChecked.Value: false));
                resultTxtBox.Text = result.SetOfSetsToString();
                resultTextArea.Text = "Result (" + result.Count.ToString() + " permutations)";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
