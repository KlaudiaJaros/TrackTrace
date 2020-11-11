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

namespace TrackTrace.Presentation
{
    /// <summary>
    /// Interaction logic for RecordContactWindow.xaml
    /// </summary>
    public partial class RecordContactWindow : Window
    {
        public RecordContactWindow()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddNewUserBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveContactBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecordAnotherBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
