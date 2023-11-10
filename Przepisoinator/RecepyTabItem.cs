using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Przepisoinator
{
    public class RecepyTabItem : TabItem
    {
        private RecepyHeader closableTabHeader;
        public string Title
        {
            set
            {
                ((RecepyHeader)Header).label_TabTitle.Content = value;
            }
        }

        protected RecepyView RecepyView;

        public RecepyTabItem(RecepyView recepyView)
        {
            // Create an instance of the usercontrol
            closableTabHeader = new RecepyHeader(this);
            // Assign the usercontrol to the tab header
            Header = closableTabHeader;
            Title = recepyView.Recepy.Name;
            // Attach to the CloseableHeader events
            // (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter +=
               new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave +=
               new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click +=
               new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged +=
               new SizeChangedEventHandler(label_TabTitle_SizeChanged);
            RecepyView = recepyView;
            AddChild(RecepyView);
        }

        void SetVisible(bool visible)
        {
            var button_close = ((RecepyHeader)Header).button_close;
            button_close.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }

        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((RecepyHeader)Header).button_close.FontWeight = FontWeights.Bold;
        }
        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((RecepyHeader)Header).button_close.FontWeight = FontWeights.Normal;
        }
        // Button Close Click - Remove the Tab - (or raise
        // an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
            RecepyView.Unind(this, RecepyView);
            ((TabControl)Parent).Items.Remove(this);
            
        }
        // Label SizeChanged - When the Size of the Label changes
        // (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((RecepyHeader)Header).button_close.Margin = new Thickness(
               ((RecepyHeader)Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }

        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            //SetVisible(true);
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            //SetVisible(false);
        }

        internal void OpenNewWindow()
        {
            RemoveLogicalChild(RecepyView);
            ((TabControl)Parent).Items.Remove(this);
            var w = new RecepyWindow(RecepyView);
            w.Show();

        }
    }
}
