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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<RecepyListViewItem> RecepieListItems;
        public MainWindow()
        {
            InitializeComponent();
            RecepieListItems = new List<RecepyListViewItem>();
            Settings.Load();
            Settings.Save();
            Directory.CreateDirectory(Settings.MainStoragePath);
            Directory.CreateDirectory(Settings.RecepyStoragePath);
            LoadRecepies();
            listView_recepies.ItemsSource = RecepieListItems;
            //BaseGrid.Children.Add(new parentRecepy());

            //MeasurementUnit.LoadAllFromDir(Settings.UnitsStoragePath);
            MeasurementUnit.SaveAllToDirectory(Settings.UnitsStoragePath);
        }

        void LoadRecepies()
        {
            RecepieListItems = new List<RecepyListViewItem>();
            foreach(var path in Directory.EnumerateFiles(Settings.RecepyStoragePath))
            {
                using var sr = new StreamReader(path);
                RecepieListItems.Add(new RecepyListViewItem(Recepy.FromJson(sr.ReadToEnd())));
            }
        }


        private void CloseTabImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var closeImage = (Image)sender;
            var tab = (TabItem)closeImage.Parent;
            mainTabControl.Items.RemoveAt(tab.TabIndex);
        }

        private void listView_recepies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecepyListViewItem? item = ((FrameworkElement)e.OriginalSource).DataContext as RecepyListViewItem;
            if (item != null)
            {
                mainTabControl.Items.Add(new RecepyTabItem(new RecepyView(item.Recepy)));
            }
        }

        private void button_addRecepy_Click(object sender, RoutedEventArgs e)
        {
            Recepy newRecepy = Recepy.GetNewRecepy();
            var recepyItemView = new RecepyListViewItem(newRecepy);
            RecepieListItems.Add(recepyItemView);
            listView_recepies.Items.Refresh();
            var newRecepyVew = new RecepyView(newRecepy);
            var newTab = new RecepyTabItem(newRecepyVew);
            mainTabControl.Items.Add(newTab);
            newTab.Focus();
            newRecepyVew.SetMode(true);
        }

        private void button_importRecepy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_exportRecepy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
