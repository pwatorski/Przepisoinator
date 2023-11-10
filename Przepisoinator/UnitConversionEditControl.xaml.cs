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
    /// Logika interakcji dla klasy UnitConversionEditControl.xaml
    /// </summary>
    public partial class UnitConversionEditControl : UserControl
    {
        public double ConversionValue { get; set; } = 1;
        public long UnitID;
        protected UnitConversionEditView ParentWindow;
        protected bool SelfConversion;
        public UnitConversionEditControl(UnitConversionEditView parentWindow, MeasurementUnit unit, double value, List<long> conversionsBlacklist, bool selfConversion)
        {
            InitializeComponent();
            ConversionValue = value;
            textBox_Value.Text = $"{ConversionValue}";
            comboBox_unit.SelectedItem = unit;
            UnitID = unit.ID;
            ParentWindow = parentWindow;
            SelfConversion = selfConversion;

            foreach (var m in MeasurementUnit.AllUnits.Values)
            {
                if (!conversionsBlacklist.Contains(m.ID) || m.ID == UnitID)
                {
                    comboBox_unit.Items.Add(m);
                }
            }
            comboBox_unit.SelectedItem = unit;
            if(SelfConversion)
            {
                textBox_Value.IsEnabled = false;
                comboBox_unit.IsEnabled = false;
            }
        }

        private void textBox_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textBox_Value.Text.Replace(".", ",");
            if (double.TryParse(text, out var amount))
            {
                ConversionValue = amount;
            }
            else if(text != "")
            {
                var selectedPos = textBox_Value.SelectionStart;
                
                textBox_Value.Text = $"{ConversionValue}";
                textBox_Value.Select(Math.Max(0,selectedPos - 1), 0);
            }
        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.RemoveConversion(this);
        }

        private void comboBox_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_unit.SelectedItem == null || ((MeasurementUnit)comboBox_unit.SelectedItem).ID == UnitID) return;
            UnitID = ((MeasurementUnit)comboBox_unit.SelectedItem).ID;
            ParentWindow.UpdateSelectedConversions();
        }

        public void UpdatePossibleConversionUnits(List<long> conversionsBlacklist)
        {
            comboBox_unit.Items.Clear();
            foreach (var m in MeasurementUnit.AllUnits.Values)
            {
                if (!conversionsBlacklist.Contains(m.ID) || m.ID == UnitID)
                {
                    comboBox_unit.Items.Add(m);
                }
            }
            comboBox_unit.SelectedItem = MeasurementUnit.AllUnits[UnitID];
        }
    }
}
