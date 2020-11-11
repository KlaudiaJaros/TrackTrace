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
    /// Interaction logic for AddLocationWindow.xaml
    /// </summary>
    public partial class AddLocationWindow : Window
    {
        public AddLocationWindow()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Location(s) successfully added.");
        }

        private void AddAnotherLocBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
