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
    /// Logika interakcji dla klasy RecepyIngredientControl.xaml
    /// </summary>
    public partial class RecepyIngredientControl : UserControl
    {
        RecepyView parentRecepy;
        public bool EditMode;
        public bool Empty { get => ingredient.Unit == MeasurementUnit.BasicUnit && textBox_name.Text.Length == 0; }
        public MeasurementUnit SUnit = MeasurementUnit.BasicUnit;
        public RecepyIngredient ingredient;
        public int Indent { get=>indent; set 
            {
                indent = value;
                if (indent > 0)
                {
                    textBlock_bulletPoint.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlock_bulletPoint.Visibility = Visibility.Collapsed;
                }
                ingredient.SetIndet(indent);
                rectangle_tabSpacer.Width = indent * IndentWidth;
            } 
        }
        protected int indent;

        public string Text { get => ingredient.Unit==MeasurementUnit.BasicUnit? ingredient.ActiveName: $"{ingredient.ActiveName} {ingredient.Value} {ingredient.Unit.ShortName}"; }
        public static readonly int IndentWidth = 20;

        public RecepyIngredientControl(RecepyView recepyView, RecepyIngredient? recepyIngredient=null)
        {
            InitializeComponent();
            parentRecepy = recepyView;
            if(recepyIngredient != null)
            {
                ingredient = recepyIngredient;
            }
            else
            {
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
                textBox_name.Text = ingredient.ActiveName;
                textBox_nameOverlay.Visibility = Visibility.Hidden;
            }
            else
            {
                textBox_nameOverlay.Visibility = Visibility.Visible;
            }
            Indent = ingredient.Indent;
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
            ingredient.Name = textBox_name.Text;
        }

        private void textBox_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox_name.Text.Length == 0)
            {
                textBox_nameOverlay.Visibility = Visibility.Visible;
            }
        }

        private void textBox_name_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox_nameOverlay.Visibility = Visibility.Hidden;
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

        private void textBox_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                parentRecepy.AddNewIngredient(this);
                e.Handled=true; 
                return;
            }
            if(e.Key == Key.V) 
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    parentRecepy.TryInsertClipboardIngredients(this);
                    e.Handled = true;
                }
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
        }

        internal void SetMode(bool editMode)
        {
            EditMode = editMode;
            if(EditMode)
            {
                stackPanel_base.Visibility = Visibility.Visible;
                Visibility = Visibility.Visible;
                textBox_fullText.Text = "";
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

        private void grid_base_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Tab) 
            {
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    if (Indent > 0)
                        Indent -= 1;
                }
                else
                {
                    Indent += 1;
                }
                e.Handled = true;
            }
        }

        private void textBox_nameOverlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(EditMode)
            {
                textBox_nameOverlay.Visibility = Visibility.Hidden;
                FocusCursor();
            }
        }

        public RecepyIngredient GetIngredient()
        {
            return ingredient;
        }
    }
}
