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
    /// Interaction logic for GenerateContactsWindow.xaml
    /// </summary>
    public partial class GenerateContactsWindow : Window
    {
        private MainWindow mainMenu;
        private List<User> users = new List<User>();
        private List<String> results = new List<String>();

        public GenerateContactsWindow()
        {
            InitializeComponent();
            users = DataFacade.GetUsers();
            datePicker.Maximum = DateTime.Now;
            datePicker.ClipValueToMinMax = true;
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }

        private void ShowAllUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            usersList.ItemsSource = users;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            usersList.ItemsSource = "";
            resultsList.ItemsSource = "";
            datePicker.Value = null;
            userInput.Text = "";
            results.Clear();
        }

        private void SearchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            List<User> userResults = new List<User>();

            if (idSearchBtn.IsChecked == true)
            {
                foreach (User u in users)
                {
                    int id = 0;
                    Int32.TryParse(userInput.Text, out id);
                    if (u.GetId() == id)
                    {
                        userResults.Add(u);
                    }
                }
            }
            else if (lastNameSearchBtn.IsChecked == true)
            {
                foreach (User u in users)
                {
                    if (u.GetLastName().ToLower().Contains(userInput.Text.ToLower()))
                    {
                        userResults.Add(u);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose if you wish to search by user's id or last name.");
            }
            usersList.ItemsSource = userResults;
        }

        private void idSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void lastNameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void ShowResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            resultsList.ItemsSource = null;
            results.Clear();

            if (datePicker.Value == null)
            {
                MessageBox.Show("Please select date and time.");
            }
            else if (usersList.SelectedItem == null)
            {
                MessageBox.Show("Please select a user.");
            }
            else
            {
                User user = (User)usersList.SelectedItem;
                List<User> users = new List<User>();
                users = DataFacade.GetUsersByContactAndDate(user.GetId(), (DateTime)datePicker.Value);

                foreach(User u in users)
                {
                    string record = u.GetFirstName() + " " + u.GetLastName() + ", phone number: " + u.GetPhoneNo();
                    results.Add(record);
                }
                resultsList.ItemsSource = results;
            }
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
