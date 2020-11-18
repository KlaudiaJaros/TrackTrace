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
        private AddUserWindow addUserWindow;
        private AddLocationWindow addLocationWindow;
        private RecordEventsWindow recordEventsWindow;
        private GenerateVisitsWindow getVisitsWindow;
        private GenerateContactsWindow getContactsWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            addUserWindow = new AddUserWindow();
            addUserWindow.Show();
            this.Close();
        }

        private void AddLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            addLocationWindow = new AddLocationWindow();
            addLocationWindow.Show();
            this.Close();
        }

        private void RecordEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            recordEventsWindow = new RecordEventsWindow();
            recordEventsWindow.Show();
            this.Close();
        }

        private void SearchContactsBtn_Click(object sender, RoutedEventArgs e)
        {
            getContactsWindow = new GenerateContactsWindow();
            getContactsWindow.Show();
            this.Close();
        }

        private void SearchLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            getVisitsWindow = new GenerateVisitsWindow();
            getVisitsWindow.Show();
            this.Close();
        }
    }
}
