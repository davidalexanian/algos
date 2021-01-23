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
    /// Interaction logic for NumericAlgorithmsWindow.xaml
    /// </summary>
    public partial class NumericAlgorithmsWindow : Window
    {
        public NumericAlgorithmsWindow()
        {
            InitializeComponent();
        }
        private void NSumOfPower2Button_Click(object sender, RoutedEventArgs e)
        {
            uint i = 0;
            if (!uint.TryParse(NSumofPower2TxtBox.Text, out i))
            {
                MessageBox.Show("Choose positive integer and try again");
                return;
            }
            var numbers = NumericAlgorithms.SumOfNumbersOfPower2(i);
            NSumOfPower2TxtBlock.Text = numbers.ToSimbolSeperatedString(" + ");
        }
        private void GCDButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultGCDTxtBlock.Text = NumericAlgorithms.GreatestCommonDivisor(uint.Parse(aTxtBox.Text), uint.Parse(bTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LCMButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultGCDTxtBlock.Text = NumericAlgorithms.LeastCommonMultiple(uint.Parse(aTxtBox.Text), uint.Parse(bTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LCMByGCDButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultGCDTxtBlock.Text = NumericAlgorithms.LeastCommonMultipleByGCD(uint.Parse(aTxtBox.Text), uint.Parse(bTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ExpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultExpTxtBlock.Text = NumericAlgorithms.ExponentBySquaring(decimal.Parse(aExpTxtBox.Text), uint.Parse(bExpTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void NRootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultExpTxtBlock.Text = NumericAlgorithms.NRoot(double.Parse(aExpTxtBox.Text), double.Parse(bExpTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void primeTestBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                resultPrimeTextBlock.Text = NumericAlgorithms.IsPrime(ulong.Parse(primeTxtBox.Text)).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void primeTestBtn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultPrimeTextBlock.Text = NumericAlgorithms.IsPrime2(ulong.Parse(primeTxtBox.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void primeTestBtn3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultPrimeTextBlock.Text = NumericAlgorithms.IsPrimeHeuristic(ulong.Parse(primeTxtBox.Text), 5).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void primeFactorsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var list = NumericAlgorithms.PrimeFactors(ulong.Parse(primeFactorsTxtBox.Text));
                if (list.Count == 0)
                {
                    MessageBox.Show("The number does not have factors");
                    return;
                }
                resultPrimeFactorsBlock.Text = Utility.ToSimbolSeperatedString(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void findPrimesBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                primeNumberstxtBox.Text = "";
                long n = long.Parse(primeFactorsTxtBox.Text);
                long[] primes = NumericAlgorithms.FindPrimes(n);

                primeNumberstxtBox.Text = Utility.ToSimbolSeperatedString(primes);
                resultPrimeFactorsBlock.Text = primes.LongLength.ToString() + " primes, approximate number N / lnN =" + (n / (long) Math.Log(n)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ConvertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                convertResultTxtBlock.Text = NumericAlgorithms.BaseConverter(numberTxtBox.Text, ushort.Parse(fromBaseTxtBox.Text), ushort.Parse(toBaseTxtBox.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ConvertSwapBtn_Click(object sender, RoutedEventArgs e)
        {
            string str = fromBaseTxtBox.Text;
            fromBaseTxtBox.Text = toBaseTxtBox.Text;
            toBaseTxtBox.Text = str;
        }
    }
}
