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
using System.Windows.Threading;

namespace Przepisoinator
{
    /// <summary>
    /// Logika interakcji dla klasy UnitEditView.xaml
    /// </summary>
    public partial class UnitEditView : UserControl
    {
        public MeasurementUnit MeasurementUnit { get; protected set; }
        UnitEditWindow ParentWindow;
        bool Focused = false;
        public bool Edited = false;
        static readonly SolidColorBrush FocusColor = new(Color.FromRgb(0xFF, 0xFF, 0xFF));
        static readonly SolidColorBrush UnFocusColor = new(Color.FromRgb(0xEE, 0xEE, 0xEE));
        static readonly SolidColorBrush HighlightColor = new(Color.FromRgb(0xF8, 0xF8, 0xF8));

        string OryginalName = string.Empty;
        string OryginalShortName10 = string.Empty;
        string OryginamName = string.Empty;

        public UnitEditView(UnitEditWindow parentWindow, MeasurementUnit measurementUnit)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            MeasurementUnit = measurementUnit;
            textBox_fullName.Text = MeasurementUnit.Name ;
            textBox_shortName.Text = MeasurementUnit.Symbol;
            if(measurementUnit.OnlyName)
            {
                checkBox_onlyName.IsChecked = true;
            }
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            MeasurementUnit.Name = textBox_fullName.Text;
            MeasurementUnit.Symbol = textBox_shortName.Text;
            MeasurementUnit.OnlyName = checkBox_onlyName.IsChecked ?? false;
        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox_shortName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Edited = true;
            ParentWindow.UpdateBar();
        }

        private void textBox_fullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Edited = true;
        }

        private void checkBox_onlyName_Checked(object sender, RoutedEventArgs e)
        {
            textBox_shortName.Visibility = Visibility.Collapsed;
            Edited = true;
        }

        private void checkBox_onlyName_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_shortName.Visibility = Visibility.Visible;
            Edited = true;
        }

        public void FocusSelect(bool focus)
        {
            Focused = focus;
            if (Focused)
            {
                SetColor(FocusColor);
            }
            else 
            { 
                SetColor(UnFocusColor); 
            }
        }

        private void grid_base_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ParentWindow.FocusOnView(this);
        }

        private void grid_base_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!Focused)
            {
                SetColor(UnFocusColor);
            }
        }

        private void grid_base_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!Focused)
            {
                SetColor(HighlightColor);
            }
        }

        void SetColor(SolidColorBrush brush)
        {
            grid_base.Background = brush;
            textBox_fullName.Background = brush;
            textBox_shortName.Background = brush;
        }

        private void grid_base_GotFocus(object sender, RoutedEventArgs e)
        {
            ParentWindow.FocusOnView(this);
        }
    }
}
