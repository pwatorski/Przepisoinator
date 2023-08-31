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
    /// Logika interakcji dla klasy IngredientView.xaml
    /// </summary>
    public partial class IngredientView : UserControl
    {
        RecepyView parentRecepy;
        protected bool EmptyName;
        public bool EditMode;
        public bool Empty = true;
        public MeasurementUnit SUnit = MeasurementUnit.BasicUnit;
        public RecepyIngredient ingredient;

        public string Text { get => $"{ingredient.ActiveName} {ingredient.Value} {ingredient.Unit.Symbol}"; }

        public IngredientView(RecepyView recepyView, RecepyIngredient? recepyIngredient=null)
        {
            InitializeComponent();
            EmptyName = true;
            parentRecepy = recepyView;
            if(recepyIngredient != null)
            {
                Empty = false;
                ingredient = recepyIngredient;
            }
            else
            {
                Empty = true;
                ingredient = RecepyIngredient.GetEmptyIngredient(); 
            }
            

            foreach (var m in MeasurementUnit.AllUnits.Values)
            {
                comboBox_unit.Items.Add(m);
            }
            comboBox_unit.SelectedItem = ingredient.Unit;
            SUnit = ingredient.Unit;
            if (ingredient.ActiveName.Length > 0)
            {
                EmptyName = false;
                textBox_name.Text = ingredient.ActiveName;
                UpdateTextMode(true);

            }
            textBox_name.GotFocus += textBox_name_GotFocus;
            textBox_name.LostFocus += textBox_name_LostFocus;
            textBox_name.TextChanged += TextBox_name_TextChanged;
        }

        public void HideRemoveButton(bool doHide)
        {
            button_remove.Visibility = doHide ? Visibility.Hidden : Visibility.Visible;
        }

        private void TextBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmptyName)
            {
                EmptyName = false;
                Empty = false;
            }
            else
            {
                if(textBox_name.Text.Length == 0)
                {
                    EmptyName = true;
                    Empty = ingredient.Unit == MeasurementUnit.BasicUnit;
                }
            }
            ingredient.Ingredient.Name = textBox_name.Text;
        }

        protected void UpdateTextMode(bool normal)
        {
            if(normal)
            {
                textBox_name.FontWeight = FontWeights.Normal;
                textBox_name.FontStyle = FontStyles.Normal;
                textBox_name.Foreground = Brushes.Black;
            }
            else
            {
                textBox_name.FontWeight = FontWeights.Light;
                textBox_name.FontStyle = FontStyles.Italic;
                textBox_name.Foreground = Brushes.Gray;
            }
        }

        private void textBox_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox_name.Text.Length == 0)
            {
                textBox_name.Text = "Dodaj składnik...";
                UpdateTextMode(false);
                EmptyName = true;
            }
        }

        private void textBox_name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmptyName)
            {
                textBox_name.Text = string.Empty;
                UpdateTextMode(true);
            }
        }

        internal void FocusCursor()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate () {
                textBox_name.Focus();
                Keyboard.Focus(textBox_name);
            }));
        }


        private void button_remove_Click(object sender, RoutedEventArgs e)
        {
            parentRecepy.RemoveIngredient(this);
        }

        private void textBox_name_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                parentRecepy.AddNewIngredient(this);
                e.Handled=true; 
                return;
            }
        }

        private void textBox_amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ingredient != null)
            {
                if (double.TryParse(textBox_amount.Text, out var amount))
                {
                    ingredient.Value = amount;
                }
                else if (textBox_amount.Text.Length > 0)
                {
                    textBox_amount.Text = $"{ingredient.Value}";
                }
            }
        }

        private void comboBox_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ingredient.ConvertInPlace((MeasurementUnit)comboBox_unit.SelectedItem);
            ingredient.Unit = (MeasurementUnit)comboBox_unit.SelectedItem;
            textBox_amount.Text = $"{ingredient.Value}";
            Empty = ingredient.Unit == MeasurementUnit.BasicUnit && EmptyName;
        }

        internal void SetMode(bool editMode)
        {
            EditMode = editMode;
            if(EditMode)
            {
                stackPanel_base.Visibility = Visibility.Visible;
                Visibility = Visibility.Visible;
            }
            else
            {
                if (Empty)
                {
                    Visibility = Visibility.Collapsed;
                }
                else
                {
                    Visibility = Visibility.Visible;
                    stackPanel_base.Visibility = Visibility.Collapsed;
                    textBox_fullText.Text = Text;
                }
            }
            
        }
    }
}
