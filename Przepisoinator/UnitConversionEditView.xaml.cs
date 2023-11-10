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
        UnitEditControl? FocusedUnitView = null;
        static readonly string WindowName = "Edycja jednostek";
        static readonly string UnsavedChangesText = "(niezapisane zmiany)";

        List<long> CurrentConversionUnitsIDs;
        public UnitConversionEditView()
        {
            InitializeComponent();

            PopulateUnits();
            CurrentConversionUnitsIDs = stackPanel_conversions.Children.OfType<UnitConversionEditControl>().Select(x => x.UnitID).ToList();
        }

        internal void FocusOnView(UnitEditControl unitEditView)
        {
            if (FocusedUnitView != null)
            {
                FocusedUnitView.FocusSelect(false);
            }
            unitEditView.FocusSelect(true);
            FocusedUnitView = unitEditView;
            RepopulateConversions();
        }

        internal void RemoveConversion(UnitConversionEditControl unitConversionEditView)
        {
            if (FocusedUnitView == null)
                return;

            FocusedUnitView.MeasurementUnit.RemoveConversion(unitConversionEditView.UnitID);
            stackPanel_conversions.Children.Remove(unitConversionEditView);
            UpdateSelectedConversions();
        }

        void PopulateUnits()
        {
            foreach (var unit in MeasurementUnit.AllUnits.Values)
            {
                var unitView = new UnitEditControl(this, unit);
                stackPanel_ingredients.Children.Add(unitView);
            }
        }

        void RepopulateConversions()
        {
            stackPanel_conversions.Children.Clear();
            if (FocusedUnitView == null) { return; }
            CurrentConversionUnitsIDs = FocusedUnitView.MeasurementUnit.Conversions.Keys.ToList();
            foreach (var convoValue in FocusedUnitView.MeasurementUnit.Conversions)
            {
                var u = MeasurementUnit.AllUnits[convoValue.Key];
                stackPanel_conversions.Children.Add(new UnitConversionEditControl(this, u, convoValue.Value, CurrentConversionUnitsIDs, u.ID == FocusedUnitView.MeasurementUnit.ID));
            }

        }

        private void button_add_ingredient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_add_conversion_Click(object sender, RoutedEventArgs e)
        {
            if (FocusedUnitView == null)
                return;
            var unit = MeasurementUnit.AllUnits.Values.First(x => !CurrentConversionUnitsIDs.Contains(x.ID));
            if (unit == null)
                return;
            stackPanel_conversions.Children.Add(new UnitConversionEditControl(this, unit, 1, CurrentConversionUnitsIDs, unit.ID == FocusedUnitView.MeasurementUnit.ID));
            UpdateSelectedConversions();
        }

        internal void UpdateBar()
        {
            bool somethingEdited = false;
            foreach (var x in stackPanel_ingredients.Children.OfType<UnitEditControl>())
            {
                somethingEdited = somethingEdited || x.Edited;
            }
            if (somethingEdited)
            {
                //Title = $"{WindowName} {UnsavedChangesText}";
            }
            else
            {
                //Title = WindowName;
            }
        }

        internal void UpdateSelectedConversions()
        {
            if (FocusedUnitView == null) return;

            CurrentConversionUnitsIDs = stackPanel_conversions.Children.OfType<UnitConversionEditControl>().Select(x => x.UnitID).ToList();
            foreach (var x in stackPanel_conversions.Children.OfType<UnitConversionEditControl>())
            {
                x.UpdatePossibleConversionUnits(CurrentConversionUnitsIDs);
            }
            if (CurrentConversionUnitsIDs.Count == MeasurementUnit.AllUnits.Count)
            {
                button_add_conversion.Visibility = Visibility.Collapsed;
            }
            else
            {
                button_add_conversion.Visibility = Visibility.Visible;
            }
        }
    }
}
