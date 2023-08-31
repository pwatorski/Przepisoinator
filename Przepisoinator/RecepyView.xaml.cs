using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy parentRecepy.xaml
    /// </summary>
    public partial class RecepyView : UserControl
    {
        public bool EditMode { get; protected set; }
        protected bool Empty;
        protected bool EmptyName;
        public bool Windowed { get; protected set; }
        public Recepy Recepy;
        public RecepyView(Recepy? recepy =null)
        {
            InitializeComponent();
            
            if (recepy == null)
                recepy = Recepy.GetEmptyRecepy();
            Recepy = recepy;
            EmptyName = true;
            foreach (var i in Recepy.Ingredients)
            {
                stackPanel_ingredients.Children.Add(new IngredientView(this, i));
            }
            stackPanel_ingredients.Children.Add(new IngredientView(this));
            wrapPanel_tags.Children.Add(new TagView(this));

            UpdateNameBox();

            textBox_name.GotFocus += textBox_name_GotFocus;
            textBox_name.LostFocus += textBox_name_LostFocus;
            textBox_name.TextChanged += TextBox_name_TextChanged;



            if (Recepy.Description.Length > 0)
            {
                Empty = false;
                //rtb_description.Document. = Recepy.Name;
                UpdateTextMode(textBox_name, true);

            }

            rtb_description.GotFocus += rtb_description_GotFocus;
            rtb_description.LostFocus += rtb_description_LostFocus;
            rtb_description.TextChanged += rtb_description_TextChanged;

            SetMode(false);
        }

        void UpdateNameBox()
        {
            if (Recepy.Name.Length > 0)
            {
                EmptyName = false;
                textBox_name.Text = Recepy.Name;
                UpdateTextMode(textBox_name, true);

            }
            else
            {
                

                if (EditMode)
                {
                    textBox_name.Text = "Podaj tytuł...";
                    EmptyName = true;
                    UpdateTextMode(textBox_name, false);
                }
                else
                {
                    textBox_name.Text = "";
                    EmptyName = true;
                    UpdateTextMode(textBox_name, false);
                }
                EmptyName = true;
            }
        }

        private void rtb_description_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var range = new TextRange(rtb_description.Document.ContentStart, rtb_description.Document.ContentEnd);
            //var stream = new MemoryStream();
            //var stream = new FileStream("text.xml", FileMode.Create);
            //range.Save(stream, "Xaml");
            //StreamReader reader = new StreamReader(stream);
            //string text = reader.ReadToEnd();
            //Console.WriteLine(text);
        }

        private void rtb_description_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void rtb_description_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmptyName)
            {
                EmptyName = false;
            }
            else
            {
                if (textBox_name.Text.Length == 0)
                {
                    EmptyName = true;
                }
            }
            Recepy.Name = textBox_name.Text;
        }

        private void textBox_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditMode)
            {
                if (textBox_name.Text.Length == 0)
                {
                    textBox_name.Text = "Podaj nazwę...";
                    UpdateTextMode(textBox_name, false);
                    textBox_name.FontWeight = FontWeights.Normal;
                    EmptyName = true;
                }
            }
        }

        private void textBox_name_GotFocus(object sender, RoutedEventArgs e)
        {
            if(EditMode)
            {
                if(EmptyName)
                {
                    textBox_name.Text = string.Empty;
                    UpdateTextMode(textBox_name, true);
                    textBox_name.FontWeight = FontWeights.Bold;
                }
            }
            else
            {

            }
        }

        private void UpdateTextMode(TextBox textBox, bool normal)
        {
            if (normal)
            {
                textBox.FontWeight = FontWeights.Normal;
                textBox.FontStyle = FontStyles.Normal;
                textBox.Foreground = Brushes.Black;
            }
            else
            {
                textBox.FontWeight = FontWeights.Light;
                textBox.FontStyle = FontStyles.Italic;
                textBox.Foreground = Brushes.Gray;
            }
        }

        internal void AddNewIngredient(IngredientView callerIngredient)
        {
            callerIngredient.HideRemoveButton(false);
            var index = stackPanel_ingredients.Children.IndexOf(callerIngredient) + 1;
            var newIngredient = new IngredientView(this);
            if (index >= stackPanel_ingredients.Children.Count) 
            {
                stackPanel_ingredients.Children.Add(newIngredient);
            }
            else
            {
                stackPanel_ingredients.Children.Insert(index, newIngredient);
            }
            newIngredient.FocusCursor();
            
        }

        internal void RemoveIngredient(IngredientView ingrediebtView)
        {
            stackPanel_ingredients.Children.Remove(ingrediebtView);
            if(stackPanel_ingredients.Children.Count == 1)
            {
                ((IngredientView)stackPanel_ingredients.Children[0]).HideRemoveButton(true);
            }
        }

        private void button_popout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        internal void AddNewTag(TagView callerTagView)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerTagView) + 1;
            var newTagView = new TagView(this);
            if (index >= wrapPanel_tags.Children.Count)
            {
                wrapPanel_tags.Children.Add(newTagView);
            }
            else
            {
                wrapPanel_tags.Children.Insert(index, newTagView);
            }
            newTagView.FocusCursor();

        }

        internal void MoveCursor(TagView callerTagView, int v)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerTagView) + v;
            if(index >= 0 && index < wrapPanel_tags.Children.Count)
            {
                ((TagView)wrapPanel_tags.Children[index]).FocusCursor(v*-1);
            }
        }

        internal void TryRemoveTag(TagView callerTagView, int v)
        {
            if(wrapPanel_tags.Children.Count < 2)
            {
                return;
            }

            var index = wrapPanel_tags.Children.IndexOf(callerTagView);
            wrapPanel_tags.Children.RemoveAt(index);
            Recepy.RemoveTag(index);
            index += v;
            index = Math.Clamp(index, 0, wrapPanel_tags.Children.Count-1);

            if (v >= 0)
                v = 1;

            ((TagView)wrapPanel_tags.Children[index]).FocusCursor(v * -1);
        }

        internal void ChangeTag(TagView callerTagView, string text)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerTagView);
            Recepy.EditTag(index, text);
        }

        private void button_editSave_Click(object sender, RoutedEventArgs e)
        {

            SetMode(!EditMode);
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            SetMode(!EditMode);
        }

        protected void SetMode(bool editMode)
        {
            EditMode = editMode;
            if (EditMode)
            {
                button_editSave.Content = "Zapisz";
                button_cancel.Visibility = Visibility.Visible;
                textBox_name.IsReadOnly = false;
                textBox_name.BorderThickness = new Thickness(1);
                rtb_description.IsReadOnly = false;
                rtb_description.BorderThickness = new Thickness(1);
            }
            else
            {
                button_editSave.Content = "Edytuj";
                button_cancel.Visibility = Visibility.Collapsed;
                textBox_name.IsReadOnly = true;
                textBox_name.BorderThickness = new Thickness(0);
                rtb_description.IsReadOnly = true;
                rtb_description.BorderThickness = new Thickness(0);
            }
            UpdateNameBox();
            foreach (var i in stackPanel_ingredients.Children)
            {
                ((IngredientView)i).SetMode(editMode);
            }
        }
    }
}
