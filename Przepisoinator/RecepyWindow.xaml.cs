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

namespace Przepisoinator
{
    /// <summary>
    /// Logika interakcji dla klasy RecepyWindoww.xaml
    /// </summary>
    public partial class RecepyWindow : Window
    {
        RecepyView RecepyView;
        public RecepyWindow(RecepyView recepyView)
        {
            InitializeComponent();
            RecepyView = recepyView;
            baseGrid.Children.Add(recepyView);
        }
    }
}
