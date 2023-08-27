﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            mainTabControl.Items.Add(new RecepyTabItem(new RecepyView())
            {
                Title = "Sól z solą"
            });
            mainTabControl.Items.Add(new RecepyTabItem(new RecepyView())
            {
                Title = "Sól z soląaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            });
            mainTabControl.Items.Add(new RecepyTabItem(new RecepyView())
            {
                Title = "Sól z solą"
            });

            //BaseGrid.Children.Add(new RecepyView());

            MeasurementUnit.SaveAllToDirectory("measurements");
        }


        private void CloseTabImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var closeImage = (Image)sender;
            var tab = (TabItem)closeImage.Parent;
            mainTabControl.Items.RemoveAt(tab.TabIndex);
        }
    }
}
