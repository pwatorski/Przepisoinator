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
    /// Logika interakcji dla klasy IngrediebtView.xaml
    /// </summary>
    public partial class IngrediebtView : UserControl
    {
        RecepyView RecepyView;
        public IngrediebtView(RecepyView recepyView)
        {
            InitializeComponent();

            RecepyView = recepyView;

            comboBox_unit.Items.Add("Kg");
            comboBox_unit.Items.Add("g");
            comboBox_unit.Items.Add("l");
            comboBox_unit.Items.Add("ml");
            comboBox_unit.Items.Add("Łyżka");
            comboBox_unit.Items.Add("Łyżeczka");
            comboBox_unit.Items.Add("Szczypta");
        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {
            RecepyView.RemoveIngredient(this);
        }

        private void textBox_name_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RecepyView.AddNewIngredient(this);
                e.Handled=true; 
                return;
            }
        }
    }
}
