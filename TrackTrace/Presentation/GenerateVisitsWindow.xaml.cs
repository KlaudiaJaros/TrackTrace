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
using System.Windows.Shapes;

namespace TrackTrace.Presentation
{
    /// <summary>
    /// Interaction logic for GenerateVisitsWindow.xaml
    /// </summary>
    public partial class GenerateVisitsWindow : Window
    {
        private MainWindow mainMenu;
        public GenerateVisitsWindow()
        {
            InitializeComponent();
        }


        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }
    }
}
