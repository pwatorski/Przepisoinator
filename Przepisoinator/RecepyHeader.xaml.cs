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
    /// Logika interakcji dla klasy CloseableHeader.xaml
    /// </summary>
    public partial class RecepyHeader : UserControl
    {
        RecepyTabItem Parent;
        public RecepyHeader(RecepyTabItem parent)
        {
            InitializeComponent();
            Parent = parent;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Parent.OpenNewWindow();
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
