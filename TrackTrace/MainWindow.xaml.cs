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
using TrackTrace.Presentation;

namespace TrackTrace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow window = new AddUserWindow();
            window.Show();
            this.Close();
        }

        private void AddLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLocationWindow window = new AddLocationWindow();
            window.Show();
            this.Close();
        }

        private void RecordContactBtn_Click(object sender, RoutedEventArgs e)
        {
            RecordContactWindow window = new RecordContactWindow();
            window.Show();
            this.Close();
        }

        private void RecordVisitBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchContactsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchLocationBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
