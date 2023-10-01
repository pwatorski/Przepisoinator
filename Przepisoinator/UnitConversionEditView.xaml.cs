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
    /// Logika interakcji dla klasy UnitConversionEditView.xaml
    /// </summary>
    public partial class UnitConversionEditView : UserControl
    {
        public double ConversionValue { get; set; } = 1;
        public long UnitID;
        protected UnitEditWindow ParentWindow;
        public UnitConversionEditView(UnitEditWindow parentWindow, MeasurementUnit unit, double value)
        {
            InitializeComponent();
            ConversionValue = value;
            textBox_Value.Text = $"{ConversionValue}";
            comboBox_unit.SelectedItem = unit;
            UnitID = unit.ID;
            ParentWindow = parentWindow;

            foreach (var m in MeasurementUnit.AllUnits.Values)
            {
                comboBox_unit.Items.Add(m);
            }
            comboBox_unit.SelectedItem = unit;
        }

        private void textBox_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(textBox_Value.Text, out var amount))
            {
                ConversionValue = amount;
            }
            else
            {
                textBox_Value.Text = $"{ConversionValue}";
            }
        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.RemoveConversion(this);
        }

        private void comboBox_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitID = ((MeasurementUnit)comboBox_unit.SelectedItem).ID;
        }
    }
}
