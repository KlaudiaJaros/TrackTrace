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
    /// Interaction logic for GenerateVisitsWindow.xaml
    /// </summary>
    public partial class GenerateVisitsWindow : Window
    {
        private List<Location> locations = new List<Location>();
        private List<String> results = new List<String>();
        

        private MainWindow mainMenu;
        public GenerateVisitsWindow()
        {
            InitializeComponent();
            locations = DataFacade.GetLocations();

            toDate.Value = DateTime.Now;
            toDate.Maximum = fromDate.Maximum = DateTime.Now;
            toDate.ClipValueToMinMax = true;
            fromDate.ClipValueToMinMax = true;

        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }

        private void ShowAllLocationsBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            locationsList.ItemsSource = locations;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            fromDate.Value = null;
            toDate.Value = null;
            locationsList.ItemsSource = null;
            userInput.Text = "";
            results.Clear();
            resultsList.ItemsSource = null;

        }

        private void SearchLocationsBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Location> locationResults = new List<Location>();

            if (postcodeSearchBtn.IsChecked==true)
            {
                foreach (Location l in locations)
                {
                    if (l.PostCode.Equals(userInput.Text))
                    {
                        locationResults.Add(l);
                    }
                }
            }
            else if (nameSearchBtn.IsChecked == true)
            {
                foreach (Location l in locations)
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

        private void ShowResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            resultsList.ItemsSource = null;
            results.Clear();

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
                Location location = (Location)locationsList.SelectedItem;
                List<User> users = new List<User>();
                users = DataFacade.GetUsersByLocationAndDate(location.GetId(), (DateTime)fromDate.Value, (DateTime)toDate.Value);

                foreach(User u in users)
                {
                    string record = u.GetFirstName() + " " + u.GetLastName() + ", phone number: " + u.GetPhoneNo();
                    results.Add(record);
                }
                resultsList.ItemsSource = results;
            }
        }

        private void postcodeSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void nameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void fromDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void toDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            fromDate.Value = null;
            fromDate.Maximum = toDate.Value;
        }

        private void ExportToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "cvs files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.FileName))
                {
                    string header = "Name, contact number:";
                    file.WriteLine(header);
                    
                    foreach (String s in results)
                    {
                        file.WriteLine(s);
                    }
                }
            }
        }
    }
}
