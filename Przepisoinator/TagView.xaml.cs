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
    /// Logika interakcji dla klasy TagView.xaml
    /// </summary>
    public partial class TagView : UserControl
    {
        protected RecepyView parentRecepy;
        bool moveLeft = false;
        bool moveRight = false;
        bool remove = false;
        public TagView()
        {
            InitializeComponent();
            parentRecepy = (RecepyView)Parent;
            textBox_name.Text = "#";
        }
        public TagView(RecepyView parent)
        {
            this.parentRecepy = parent;
            InitializeComponent();
            textBox_name.Text = "#";

        }

        private void textBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_name.Text.Length == 0 || textBox_name.Text[0] != '#')
            {
                textBox_name.Text = $"#{textBox_name.Text}";
                textBox_name.Select(1, 0);
                return;
            }
            if(textBox_name.Text.EndsWith("  "))
            {
                parentRecepy.AddNewTag(this);
                textBox_name.Text = textBox_name.Text.TrimEnd();
            }
            parentRecepy.ChangeTag(this, textBox_name.Text);
        }

        private void textBox_name_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox_name.Text = $"#{textBox_name.Text.Substring(1).Trim()}";
            moveRight = false;
            moveLeft = false;
            remove = false;
        }

        private void textBox_name_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    parentRecepy.AddNewTag(this);
                    e.Handled = true;
                    return;
                case Key.Left:
                    if(textBox_name.SelectionLength == 0 && textBox_name.SelectionStart <=1)
                    {
                        if (moveLeft || textBox_name.Text.Length == 1)
                        {
                            moveLeft = false;
                            parentRecepy.MoveCursor(this, -1);
                        }
                        else
                            moveLeft = true;
                        return;
                    }
                    else
                    {
                        moveLeft = false;
                    }
                    break;
                case Key.Right:
                    if (textBox_name.SelectionLength == 0 && textBox_name.SelectionStart >= textBox_name.Text.Length)
                    {
                        if (moveRight || textBox_name.Text.Length == 1)
                        {
                            parentRecepy.MoveCursor(this, 1);
                            moveRight = false;
                        }
                        else
                            moveRight = true;
                        return;
                    }
                    else
                    {
                        moveRight = false;
                    }
                    break;
                case Key.Back:
                    if (textBox_name.SelectionLength == 0 && textBox_name.SelectionStart <= 1 && textBox_name.Text.Length == 1)
                    {
                        if (remove)
                        {
                            parentRecepy.TryRemoveTag(this, -1);
                            remove = false;
                        }
                        else
                        {
                            remove = true;
                        }
                        return;
                    }
                    break;
                case Key.Delete:
                    if (textBox_name.SelectionLength == 0 && textBox_name.SelectionStart <= 1 && textBox_name.Text.Length == 1)
                    {
                        if (remove)
                        {
                            parentRecepy.TryRemoveTag(this, 0);
                            remove = false;
                        }
                        else
                        {
                            remove = true;
                        }
                        return;
                    }
                    break;


            }
        }

        internal void FocusCursor(int side = 0)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate () {
                textBox_name.Focus();
                Keyboard.Focus(textBox_name);
                if (side < 0) 
                {
                    textBox_name.Select(0,0);
                }
                else if (side > 0)
                {
                    textBox_name.Select(textBox_name.Text.Length, 0);
                }
            }));
        }

        private void textBox_name_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(textBox_name.SelectionStart == 0)
            {
                textBox_name.SelectionStart = 1;
                textBox_name.SelectionLength = Math.Max(0, textBox_name.SelectionLength - 1);
            }
        }
    }
}
