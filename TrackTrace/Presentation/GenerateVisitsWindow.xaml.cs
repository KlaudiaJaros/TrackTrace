using Microsoft.Win32;
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
using TrackTrace.BusinessObject;
using TrackTrace.Data;

namespace TrackTrace.Presentation
{
    /// <summary>
    /// Interaction logic for GenerateVisitsWindow.xaml. The window displays an interactive form where the user can search through locations 
    /// and generate a list of users who visited a given location between choosen dates and times. The list can be exported to a file.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    public partial class GenerateVisitsWindow : Window
    {
        private MainWindow mainMenu;
        private List<Location> _locations = new List<Location>(); // store all locations from the data layer
        private List<User> _userResults = new List<User>(); // store search results
        

        public GenerateVisitsWindow()
        {
            InitializeComponent();

            // get all locations from the data layer:
            _locations = DataFacade.GetLocations();

            // set-up DatePickers:
            toDate.Value = DateTime.Now;
            toDate.Maximum = fromDate.Maximum = DateTime.Now;
            toDate.ClipValueToMinMax = true;
            fromDate.ClipValueToMinMax = true;
        }
        /// <summary>
        /// Returns to Main Window.
        /// </summary>
        /// <param name="sender">'Return' button</param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Displays all locations that are in the system in the location list control.
        /// </summary>
        /// <param name="sender">'Show all' button</param>
        /// <param name="e"></param>
        private void ShowAllLocationsBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            locationsList.ItemsSource = _locations;
        }

        /// <summary>
        /// Clears the form for a new search.
        /// </summary>
        /// <param name="sender">'Clear' button</param>
        /// <param name="e"></param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            fromDate.Value = null;
            toDate.Value = null;
            locationsList.ItemsSource = null;
            userInput.Text = "";
            _userResults.Clear();
            resultsList.ItemsSource = null;
        }

        /// <summary>
        /// Allows a user to search for a specific location. If the post-code search radio button is checked, it searches by a post-code, if the name button is checked,
        /// it searches by the location's name.
        /// </summary>
        /// <param name="sender">'Search' button</param>
        /// <param name="e"></param>
        private void SearchLocationsBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Location> locationResults = new List<Location>(); // store location search results

            if (postcodeSearchBtn.IsChecked==true)
            {
                foreach (Location l in _locations)
                {
                    if (l.PostCode.Equals(userInput.Text))
                    {
                        locationResults.Add(l);
                    }
                }
            }
            else if (nameSearchBtn.IsChecked == true)
            {
                foreach (Location l in _locations)
                {
                    if (l.Name.ToLower().Contains(userInput.Text.ToLower()))
                    {
                        locationResults.Add(l);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose if you wish to search by post code or name.");
            }
            locationsList.ItemsSource = locationResults;
        }

        /// <summary>
        /// Displays all users that were recorded in the specified location between specified date and time.
        /// </summary>
        /// <param name="sender">'Show results' button</param>
        /// <param name="e"></param>
        private void ShowResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            // clear previous results:
            resultsList.ItemsSource = null;
            _userResults.Clear();

            if (fromDate.Value == null || toDate.Value == null)
            {
                MessageBox.Show("Please select dates.");
            }
            else if (locationsList.SelectedItem==null)
            {
                MessageBox.Show("Please select a location.");
            }
            else
            {
                // get location object selected by the user:
                Location location = (Location)locationsList.SelectedItem;

                // get all users that match the search:
                _userResults = DataFacade.GetUsersByLocationAndDate(location.ID, (DateTime)fromDate.Value, (DateTime)toDate.Value);
                resultsList.ItemsSource = _userResults; // display results;
            }
        }

        /// <summary>
        /// Ensures that the "from date" cannot be set later than the "to date". "From date" should always be earlier than "to date".
        /// </summary>
        /// <param name="sender">User changed "to date" value.</param>
        /// <param name="e"></param>
        private void toDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            fromDate.Value = null;
            fromDate.Maximum = toDate.Value;
        }

        /// <summary>
        /// Exports the results into a CSV file and let's the user choose where to save it. 
        /// </summary>
        /// <param name="sender">'Export to a file' button</param>
        /// <param name="e"></param>
        private void ExportToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            // set-up a file dialog to save the results:
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "cvs files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                // save the results to a file in a choosen directory with a choosen name:
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.FileName))
                {
                    string header = "User, contact number, (optional) name:";
                    file.WriteLine(header);
                    
                    foreach (User u in _userResults)
                    {
                        file.WriteLine(u.ToString());
                    }
                }
            }
        }

        // methods below reset the user input field if the search parameters change:
        private void postcodeSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void nameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }
    }
}
