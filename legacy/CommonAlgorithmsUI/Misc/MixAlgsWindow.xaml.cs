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
using CommonAlgorithms.String;

namespace CommonAlgorithmsUI
{
    /// <summary>
    /// Interaction logic for StringWindow.xaml
    /// </summary>
    public partial class MixAlgorithmsWindow : Window
    {
        public MixAlgorithmsWindow()
        {
            InitializeComponent();
        }

        private void EvaluteAritExpression_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var exp = ExpressiontxtBox.Text;
                double r = StringAlgorithms.EvaluateArithmeticExpression(exp);
                EvaluatedExpResult.Text = r.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
