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
    /// Logika interakcji dla klasy RecepyView.xaml
    /// </summary>
    public partial class RecepyView : UserControl
    {
        public bool EditMode { get; protected set; }
        public bool Windowed { get; protected set; }
        public Recepy Recepy;
        public RecepyView(Recepy recepy=null)
        {
            InitializeComponent();
            stackPanel_ingredients.Children.Add(new IngrediebtView(this));
            if (recepy == null)
                recepy = Recepy.BasicRecepy;
            Recepy = recepy;
        }

        internal void AddNewIngredient(IngrediebtView ingrediebtView)
        {
            var index = stackPanel_ingredients.Children.IndexOf(ingrediebtView) + 1;
            if (index >= stackPanel_ingredients.Children.Count) 
            {
                stackPanel_ingredients.Children.Add(new IngrediebtView(this));
            }
            else
            {
                stackPanel_ingredients.Children.Insert(index, new IngrediebtView(this));
            }
        }

        internal void RemoveIngredient(IngrediebtView ingrediebtView)
        {
            stackPanel_ingredients.Children.Remove(ingrediebtView);
        }

        private void button_popout_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
