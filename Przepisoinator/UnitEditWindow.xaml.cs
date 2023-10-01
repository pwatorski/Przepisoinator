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
using System.Windows.Threading;

namespace Przepisoinator
{
    /// <summary>
    /// Logika interakcji dla klasy UnitEditWindow.xaml
    /// </summary>
    public partial class UnitEditWindow : Window
    {
        UnitEditView? FocusedUnitView = null;
        public UnitEditWindow()
        {
            InitializeComponent();

            PopulateUnits();
        }

        internal void FocusOnView(UnitEditView unitEditView)
        {
            if(FocusedUnitView != null)
            {
                FocusedUnitView.FocusSelect(false);
            }
            unitEditView.FocusSelect(true);
            FocusedUnitView = unitEditView;
            RepopulateConversions();
        }

        internal void RemoveConversion(UnitConversionEditView unitConversionEditView)
        {
            if (FocusedUnitView == null)
                return;

            FocusedUnitView.MeasurementUnit.RemoveConversion(unitConversionEditView.UnitID);
            stackPanel_conversions.Children.Remove(unitConversionEditView);
        }

        void PopulateUnits()
        {
            foreach (var unit in MeasurementUnit.AllUnits.Values)
            {
                var unitView = new UnitEditView(this, unit);
                stackPanel_ingredients.Children.Add(unitView);
            }
        }

        void RepopulateConversions()
        {
            stackPanel_conversions.Children.Clear();
            if(FocusedUnitView == null) { return; }
            foreach(var convoValue in FocusedUnitView.MeasurementUnit.Conversions)
            {
                var u = MeasurementUnit.AllUnits[convoValue.Key];
                stackPanel_conversions.Children.Add(new UnitConversionEditView(this, u, convoValue.Value));
            }
        }

        private void button_add_ingredient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_add_conversion_Click(object sender, RoutedEventArgs e)
        {
            if (FocusedUnitView == null)
                return;
            stackPanel_conversions.Children.Add(new UnitConversionEditView(this, MeasurementUnit.BasicUnit, 1));
        }

        internal void UpdateBar()
        {
            throw new NotImplementedException();
        }
    }
}
