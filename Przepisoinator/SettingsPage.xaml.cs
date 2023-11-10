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
    /// Logika interakcji dla klasy UnitConversionEditView.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
            textBox_appDir.Text = Settings.MainStoragePath;
        }

        private void button_saveAppDir_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.InitialDirectory = Settings.MainStoragePath;
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Settings.MainStoragePath = fbd.SelectedPath;
                    textBox_appDir.Text = Settings.MainStoragePath;
                    Settings.Save();
                }

            }
        }
    }
}
