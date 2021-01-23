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
using System.ComponentModel;
using CommonAlgorithms;
using CommonAlgorithms.Numerical;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for RandomWindow.xaml
    /// </summary>
    public partial class RandomNumbersWindow : Window, INotifyPropertyChanged
    {
        private CongruentalRandomNumberGenerator rndGen;
        private long generatedCount = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public RandomNumbersWindow()
        {
            InitializeComponent();

            txtBlockGeneratedCount.DataContext = this;
        }
        public long GeneratedCount
        {
            get { return this.generatedCount; }
            set
            {
                this.generatedCount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GeneratedCount"));
            }
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (rdButtonParameters.IsChecked.HasValue && rdButtonParameters.IsChecked.Value)
            {
                ushort A;
                uint seed, M, B;

                if (!ushort.TryParse(txtBoxA.Text, out A))
                {
                    MessageBox.Show("Value for A is incorrect.Use valid ushort value.");
                    return;
                }
                if (!uint.TryParse(txtBoxB.Text, out B))
                {
                    MessageBox.Show("Value for B is incorrect.Use valid uint value.");
                    return;
                }
                if (!uint.TryParse(txtBoxSeed.Text, out seed))
                {
                    MessageBox.Show("Value for seed is incorrect.Use valid uint value.");
                    return;
                }
                if (!uint.TryParse(txtBoxM.Text, out M))
                {
                    MessageBox.Show("Value for M is incorrect.Use valid uint value.");
                    return;
                }
                this.rndGen = new CongruentalRandomNumberGenerator(A, B, seed, M);
            }
            else
                this.rndGen = new CongruentalRandomNumberGenerator();
            GeneratedCount = 0;
            btnGenerate.IsEnabled = true;
            btnReset.IsEnabled = true;
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (rndGen != null) rndGen.Reset();
            GeneratedCount = 0;
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (this.rndGen == null)
            {
                MessageBox.Show("First create random number generator");
                return;
            }
            // Next
            if (rdBtnGenNext.IsChecked.HasValue && rdBtnGenNext.IsChecked.Value)
            {
                this.txtBoxGeneratedNumber.Text = rndGen.Next().ToString();
                this.GeneratedCount++;
                return;
            }
            // NextDouble
            if (rdBtnGenNextDbl.IsChecked.HasValue && rdBtnGenNextDbl.IsChecked.Value)
            {
                this.txtBoxGeneratedNumber.Text = rndGen.NextDouble().ToString();
                this.GeneratedCount++;
                return;
            }
            // Next range
            if (rdBtnGenNextRange.IsChecked.HasValue && rdBtnGenNextRange.IsChecked.Value)
            {
                int min, max;
                if (!int.TryParse(txtBoxMin.Text, out min) || !int.TryParse(txtBoxMax.Text, out max))
                {
                    MessageBox.Show("Make sure min & max values are correct int values and try again");
                    return;
                }
                this.txtBoxGeneratedNumber.Text = rndGen.Next(min,max).ToString();
                this.GeneratedCount++;
                return;
            }
            // Sequence N
            if (rdBtnGenNextSeq.IsChecked.HasValue && rdBtnGenNextSeq.IsChecked.Value)
            {
                int n;
                if (!GetN(out n)) return;

                uint[] sequence = rndGen.GenerateSequence(n);
                AssignSequenceToTextBox(sequence);
                GeneratedCount += sequence.Length;
            }
            // Sequence N from range
            if (rdBtnGenNextSeqRange.IsChecked.HasValue && rdBtnGenNextSeqRange.IsChecked.Value)
            {
                int min, max,n;
                if (!GetMinMaxAndN(out min, out max, out n)) return;

                int[] sequence = rndGen.GenerateSequence(n, min, max);
                AssignSequenceToTextBox(sequence);
                GeneratedCount += sequence.Length;
            }
        }
        private void btnContainsDuplicates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long reps = 0;                
                string[] values = txtBoxSequence.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, long> repeatitions = new Dictionary<string, long>(values.Length);
                foreach (var item in values)
                {
                    if (repeatitions.ContainsKey(item))
                        reps++;
                    else
                        repeatitions.Add(item, 0);
                }
                MessageBox.Show(string.Format("{0} repeatitions out of {1} numbers",reps,values.LongLength));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxSequence.Text = "";
        }
        private void btnGenNETRandom_Click(object sender, RoutedEventArgs e)
        {
            int min, max, n;
            if (!GetMinMaxAndN(out min, out max, out n)) return;

            Random r = new Random();
            int[] sequence = new int[n];
            for (int i = 0; i <= n - 1; i++)
            {
                sequence[i] = r.Next(min, max);
            }
            AssignSequenceToTextBox(sequence);
        }
        private void btnRandomize_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxSequence.Text)) return;
            string[] data = txtBoxSequence.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            CongruentalRandomNumberGenerator.RandomizeInput<string>(data);
            AssignSequenceToTextBox(data);
        }
        private void btnDistrClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
        private void btnDistr_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            if (string.IsNullOrEmpty(txtBoxSequence.Text)) return;
            string[] data = txtBoxSequence.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;
            uint number = 0;
            int value = 0;
            System.Windows.Shapes.Line l;

            // create a line for each number
            foreach (var item in data)
            {
                if (uint.TryParse(item, out number))
                {
                    value = Utility.FitNumberIntoRange(0, width, number);
                    l = new System.Windows.Shapes.Line();
                    l.Stroke = Brushes.Red;
                    l.StrokeThickness = 0.1;
                    l.X1 = value;
                    l.X2 = value;
                    l.Y1 = 0;
                    l.Y2 = height;
                    canvas.Children.Add(l);
                }                
            }
        }

        private void btnGenNETCrypto_Click(object sender, RoutedEventArgs e)
        {
            int min, max, n;
            if (!GetMinMaxAndN(out min, out max, out n)) return;

            System.Security.Cryptography.RNGCryptoServiceProvider sp = new System.Security.Cryptography.RNGCryptoServiceProvider();
            int[] sequence = new int[n];
            int counter = 0, number;
            byte[] numberBytes = new byte[4];
            while (counter < n)
            {
                sp.GetBytes(numberBytes);
                number = BitConverter.ToInt32(numberBytes, 0);
                sequence[counter] = FitNumberIntoRange(min, max, number);
                counter++;
            };
            AssignSequenceToTextBox(sequence);
        }

        // utility functions
        private bool GetMinMaxAndN(out int min, out int max, out int n)
        {
            min = 0;
            max = 0;
            n = 0;
            if (!GetMinMax(out min, out max)) return false;
            if (!GetN(out n)) return false;
            return true;
        }
        private bool GetMinMax(out int min, out int max)
        {
            min = 0;
            max = 0;
            if (!int.TryParse(txtBoxMin.Text, out min) || !int.TryParse(txtBoxMax.Text, out max))
            {
                MessageBox.Show("Make sure min & max values are correct int values and try again");
                return false;
            }
            if (min >= max)
            {
                MessageBox.Show("Make sure min is smaller than max");
                return false;
            }            
            return true;

        }
        private bool GetN(out int n)
        {
            if (!int.TryParse(txtBoxN.Text, out n))
            {
                MessageBox.Show("Make sure n is valid nonnegative integer greater or equal 1");
                return false;
            }
            if (n < 1)
            {
                MessageBox.Show("Make sure n is valid nonnegative integer greater or equal 1");
                return false;
            }
            return true;
        }
        private void AssignSequenceToTextBox(System.Collections.IEnumerable collection)
        {
            StringBuilder sb = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            foreach (var item in collection)
            {
                sb.Append(item.ToString() + ",");
            }
            sb.Replace(",", "", sb.Length - 1, 1);
            txtBoxSequence.Text = sb.ToString();
        }
        private int FitNumberIntoRange(int min, int max, int value)
        {
            int charsCount = value.ToString().Length;
            if (value < 0) charsCount--;
            double d1 = System.Math.Abs((double)value);
            double d2 = Math.Pow(10, charsCount);
            double d3 = (d1 / d2) * (max - min);
            return min + (int)d3;
        }
    }    
}
