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

namespace Przepisoinator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Recepy> items = new List<Recepy>
            {
                new Recepy() { Name = "Complete this WPF tutorial" },
                new Recepy() { Name = "Learn C#" },
                new Recepy() { Name = "Wash the car" }
            };

            listBox_recepies.ItemsSource = items;

            MeasurementUnit.SaveAllToDirectory("measurements");
        }


        private void rtb_test_TestChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(
                rtb_test.Document.ContentStart,
                rtb_test.Document.ContentEnd
            );

            Console.WriteLine(textRange.Text);
        }
    }
}
