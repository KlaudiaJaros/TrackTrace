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
    /// Interaction logic for GenerateContactsWindow.xaml. The window displays an interactive form where the user can search through contacts 
    /// and generate a list of users who were in contact with a given user after a specified time. The list can be exported to a file.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    public partial class GenerateContactsWindow : Window
    {
        private MainWindow _mainMenu;
        private Dictionary<long, User> _users = new Dictionary<long, User>(); // to store all users from the data layer
        private List<User> _userResults = new List<User>(); // to store contacts search results

        public GenerateContactsWindow()
        {
            InitializeComponent();
            
            // get all users from the data layer:
            _users = DataFacade.GetUsers();

            // set-up DateTime picker:
            datePicker.Maximum = DateTime.Now;
            datePicker.ClipValueToMinMax = true;
        }
        // TODO: get rid of double results 

        /// <summary>
        /// Returns to Main Window.
        /// </summary>
        /// <param name="sender">'Return' button</param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenu = new MainWindow();
            _mainMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Displays all users saved in the system.
        /// </summary>
        /// <param name="sender">'Show all' button</param>
        /// <param name="e"></param>
        private void ShowAllUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            usersList.ItemsSource = _users;
        }

        /// <summary>
        /// Clears the form for the next search
        /// </summary>
        /// <param name="sender">'Clear' button</param>
        /// <param name="e"></param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            usersList.ItemsSource = "";
            resultsList.ItemsSource = "";
            datePicker.Value = null;
            userInput.Text = "";
            _userResults.Clear();
        }

        /// <summary>
        /// Allows a user to search for a specific user, if search by id button is checked it searches by id, if search by last name button is checked, it searches by last name.
        /// </summary>
        /// <param name="sender">'Search' button</param>
        /// <param name="e"></param>
        private void SearchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            // search for a user that other users were in contact with:
            Dictionary<long,User> foundUsers = new Dictionary<long, User>();

            if (idSearchBtn.IsChecked == true)
            {
                long.TryParse(userInput.Text, out long id);
                _users.TryGetValue(id, out User findUser); // TryGet because if such user doesn't exist, it throws and error
                foundUsers.Add(id,findUser);
            }
            else if (lastNameSearchBtn.IsChecked == true)
            {
                foreach (KeyValuePair<long,User> u in _users)
                {
                    if (u.Value.LastName.ToLower().Contains(userInput.Text.ToLower()))
                    {
                        foundUsers.Add(u.Key,u.Value);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose if you wish to search by user's id or last name.");
            }
            usersList.ItemsSource = foundUsers;
        }


        /// <summary>
        /// Displays all users that were in contact with a specified user after a choosen date and time.
        /// </summary>
        /// <param name="sender">'Show results' button</param>
        /// <param name="e"></param>
        private void ShowResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            resultsList.ItemsSource = null;
            _userResults.Clear();

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
                // get the User object choosen by the user:
                User user = (User)((KeyValuePair<long, User>)usersList.SelectedItem).Value; // user selection is a KeyValuePair

                // find all users that match the search:
                _userResults = DataFacade.GetUsersByContactAndDate(user.ID, (DateTime)datePicker.Value);
                resultsList.ItemsSource = _userResults; // display
            }
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
                // save results to a file using the choosen direcotry and file name:
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
        private void idSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }

        private void lastNameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
        }
    }
}
