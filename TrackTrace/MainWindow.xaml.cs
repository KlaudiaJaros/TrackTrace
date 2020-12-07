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
    /// Interaction logic for MainWindow.xaml. Displays the main menu for the application.
    /// Created by: Klaudia Jaros
    /// Last modified: 06/12/2020
    /// </summary>
    public partial class MainWindow : Window
    {
        // navigation windows:
        private AddUserWindow _addUserWindow;
        private AddLocationWindow _addLocationWindow;
        private RecordEventsWindow _recordEventsWindow;
        private GenerateVisitsWindow _getVisitsWindow;
        private GenerateContactsWindow _getContactsWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        // depending on which button the user chooses, the apropriate window will open:

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            _addUserWindow = new AddUserWindow();
            _addUserWindow.Show();
            this.Close();
        }

        private void AddLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            _addLocationWindow = new AddLocationWindow();
            _addLocationWindow.Show();
            this.Close();
        }

        private void RecordEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            _recordEventsWindow = new RecordEventsWindow();
            _recordEventsWindow.Show();
            this.Close();
        }

        private void SearchContactsBtn_Click(object sender, RoutedEventArgs e)
        {
            _getContactsWindow = new GenerateContactsWindow();
            _getContactsWindow.Show();
            this.Close();
        }

        private void SearchLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            _getVisitsWindow = new GenerateVisitsWindow();
            _getVisitsWindow.Show();
            this.Close();
        }
    }
}
