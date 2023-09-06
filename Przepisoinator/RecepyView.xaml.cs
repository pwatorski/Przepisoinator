using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
    /// Logika interakcji dla klasy parentRecepy.xaml
    /// </summary>
    public partial class RecepyView : UserControl
    {
        public bool EditMode { get; protected set; }
        protected bool Empty;
        public bool Windowed { get; protected set; }
        public Recepy Recepy;
        public RecepyView(Recepy? recepy =null)
        {
            InitializeComponent();
            
            if (recepy == null)
                recepy = Recepy.GetEmptyRecepy();
            Recepy = recepy;

            if(Recepy.Name.Length > 0)
            {
                textBox_name.Text = Recepy.Name;
            }

            foreach (var i in Recepy.Ingredients)
            {
                stackPanel_ingredients.Children.Add(new IngredientView(this, i));
            }

            foreach (var t in Recepy.Tags)
            {
                wrapPanel_tags.Children.Insert(wrapPanel_tags.Children.Count - 1, new TagView(this, t));
            }
            stackPanel_ingredients.Children.Add(new IngredientView(this));
            //wrapPanel_tags.Children.Add(new TagView(this));

            UpdateNameBox();

            textBox_name.GotFocus += textBox_name_GotFocus;
            textBox_name.LostFocus += textBox_name_LostFocus;
            textBox_name.TextChanged += TextBox_name_TextChanged;



            if (Recepy.DescriptionFlow != null)
            {
                Empty = false;
                rtb_description.Document = Recepy.DescriptionFlow;
                UpdateTextMode(textBox_name, true);

            }

            rtb_description.GotFocus += rtb_description_GotFocus;
            rtb_description.LostFocus += rtb_description_LostFocus;
            rtb_description.TextChanged += rtb_description_TextChanged;

            SetMode(false);
        }

        void UpdateNameBox()
        {

        }

        protected bool DescEmpty()
        {

            if (rtb_description.Document.Blocks.Count == 0) return true;
                TextPointer startPointer = rtb_description.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
                TextPointer endPointer = rtb_description.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return startPointer.CompareTo(endPointer) == 0;
            
        }

        private void rtb_description_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Recepy.De = textBox_name.Text;
        }

        private void rtb_description_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditMode && DescEmpty())
            {
                rtb_descriptionOverlay.Visibility = Visibility.Visible;
            }
        }

        private void rtb_description_GotFocus(object sender, RoutedEventArgs e)
        {
            rtb_descriptionOverlay.Visibility = Visibility.Hidden;
        }

        private void TextBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Recepy.Name = textBox_name.Text;
            Empty = textBox_name.Text.Length == 0;
        }

        private void textBox_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditMode && textBox_name.Text.Length == 0)
            {
                textBox_nameOverlay.Visibility = Visibility.Visible;
            }
        }

        private void textBox_name_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox_nameOverlay.Visibility = Visibility.Hidden;
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

        internal void AddNewTag(Control callerControl)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerControl) + 1;
            var newTagView = new TagView(this);
            if (index >= wrapPanel_tags.Children.Count)
            {
                wrapPanel_tags.Children.Insert(index - 1, newTagView);
            }
            else
            {
                wrapPanel_tags.Children.Insert(index, newTagView);
            }
            newTagView.FocusCursor();
            newTagView.SetMode(EditMode);

        }

        internal void MoveCursor(TagView callerTagView, int v)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerTagView) + v;
            if(index >= 0 && index < wrapPanel_tags.Children.Count-1)
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
            if (wrapPanel_tags.Children.Count > 1)
            {
                ((TagView)wrapPanel_tags.Children[index]).FocusCursor(v * -1);
            }
        }

        internal void ChangeTag(TagView callerTagView, string text)
        {
            var index = wrapPanel_tags.Children.IndexOf(callerTagView);
            Recepy.EditTag(index, text);
        }

        private void button_editSave_Click(object sender, RoutedEventArgs e)
        {
            if(EditMode)
            {
                Save();
            }
            SetMode(!EditMode);
        }

        private void Save()
        {
            Recepy.Ingredients = stackPanel_ingredients.Children.OfType<IngredientView>().Where(x=>!x.Empty).Select(x => x.GetIngredient()).ToList();
            Recepy.Tags = wrapPanel_tags.Children.OfType<TagView>().Select(x => x.textBox_name.Text).ToList();
            Recepy.DescriptionFlow = rtb_description.Document;

            foreach (var b in rtb_description.Document.Blocks) 
            {
                Console.WriteLine(b.GetType());
                if(b.GetType() == typeof(Paragraph)) 
                {
                    Paragraph p = (Paragraph)b;
                    foreach(var i in p.Inlines)
                    {
                        Console.WriteLine(" " + i.GetType().ToString());
                    }
                }
            }

            using (var sw = new StreamWriter($"test_rtb.json"))
            {
                sw.Write(Recepy.ToJson());
            }
            ;
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

                textBox_nameOverlay.Visibility = textBox_name.Text.Length > 0 ? Visibility.Hidden : Visibility.Visible;
                rtb_descriptionOverlay.Visibility = DescEmpty() ? Visibility.Visible : Visibility.Hidden;
                rtb_description.IsReadOnly = false;
                rtb_description.BorderThickness = new Thickness(1);

                button_addTag.Visibility = Visibility.Visible;
                textBlock_tags.Visibility = Visibility.Visible;
            }
            else
            {
                button_editSave.Content = "Edytuj";
                button_cancel.Visibility = Visibility.Collapsed;
                textBox_name.IsReadOnly = true;
                textBox_name.BorderThickness = new Thickness(0);
                textBox_nameOverlay.Visibility = Visibility.Hidden;
                rtb_descriptionOverlay.Visibility = Visibility.Hidden;
                rtb_description.IsReadOnly = true;
                rtb_description.BorderThickness = new Thickness(0);
                button_addTag.Visibility = Visibility.Hidden;
                textBlock_tags.Visibility = Visibility.Collapsed;
            }
            UpdateNameBox();
            foreach (var i in stackPanel_ingredients.Children.OfType<IngredientView>())
            {
                i.SetMode(editMode);
            }

            foreach (var i in wrapPanel_tags.Children.OfType<TagView>())
            {
                i.SetMode(editMode);
            }
        }
        void FocusCursor(Control target)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate () {
                target.Focus();
                Keyboard.Focus(target);
            }));
        }
        private void textBox_nameOverlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (EditMode)
            {
                textBox_nameOverlay.Visibility = Visibility.Hidden;
                FocusCursor(textBox_name);
            }
        }

        private void rtb_descriptionOverlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (EditMode)
            {
                rtb_descriptionOverlay.Visibility = Visibility.Hidden;
                FocusCursor(rtb_description);
            }
        }

        private void button_addTag_Click(object sender, RoutedEventArgs e)
        {
            AddNewTag(button_addTag);
        }

        internal void TryInsertClipboardIngredients(IngredientView callerIngredient)
        {
            var index = stackPanel_ingredients.Children.IndexOf(callerIngredient);

            if(Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                var lines = text.Replace("\r", "").Split("\n").Where(x=>x.Length > 1);
                bool success = false;
                foreach(var l in lines)
                {
                    var words = l.Split(' ');
                    if(words.Length > 1)
                    {
                        double value = 1;
                        bool foundValue = false;
                        MeasurementUnit unit = MeasurementUnit.BasicUnit;
                        int wordCount = words.Length;
                        int wordNum;
                        var last = words.Last();

                        for(wordNum = wordCount - 1; wordNum >= 0 && !foundValue; wordNum--)
                        {
                            Console.WriteLine(words[wordNum]);
                            if (double.TryParse(words[wordNum], out value))
                            {
                                foundValue = true;
                                success = true;
                                break;
                            }
                        }
                        if(!foundValue)
                        {
                            wordNum = words.Length-1;
                        }

                        Console.WriteLine($"wordNum: {wordNum}");

                        var name = string.Join(" ", words[0..wordNum]);
                        Console.WriteLine($"name: {name}");

                        var unitName = string.Join(" ", words[(wordNum+1)..]);
                        Console.WriteLine($"unitName: {unitName}");
                        unit = MeasurementUnit.FindMostSimilar(unitName);
                        index += 1;
                        stackPanel_ingredients.Children.Insert(index,
                            new IngredientView(this, new RecepyIngredient(name, unit, value)));
                    }
                }
                if (success)
                {
                    stackPanel_ingredients.Children.Remove(callerIngredient);
                }
            }

        }
    }
}
