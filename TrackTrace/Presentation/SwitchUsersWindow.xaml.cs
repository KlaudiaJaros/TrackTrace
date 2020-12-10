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
    /// Interaction logic for SwitchUsersWindow.xaml. Allows a user to change the default user.
    /// Created by: Klaudia Jaros
    /// Last modified: 09/12/2020
    /// </summary>
    public partial class SwitchUsersWindow : Window
    {
        private Dictionary<long, User> _users = new Dictionary<long, User>();
        private RecordEventsWindow _recordEventWindow;
        public SwitchUsersWindow()
        {
            InitializeComponent();
            usersList.SelectionMode = SelectionMode.Single;

            // load data from the data layer:
            LoadData();
        }
        /// <summary>
        /// Loads data for users and locations using DataFacade.
        /// </summary>
        private void LoadData()
        {
            _users = DataFacade.GetUsers();

            User defaultUser = DataFacade.GetDefaultUser();
            if (defaultUser != null) // set the default user if there is one
            {
                currentUserBox.Text = defaultUser.ToString();
            }
        }
        /// <summary>
        /// Searches through all users either by id or last name and displays the result in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            string search = searchUsersInput.Text;
            Dictionary<long, User> results = new Dictionary<long, User>();

            if (UserIDSearchBtn.IsChecked==true)
            {
                long id = 0;
                long.TryParse(search, out id); // parse id to long
                _users.TryGetValue(id, out User u); // check if it's in the dictionary
                results.Add(id, u); // save if found
            }
            else
            {
                foreach (KeyValuePair<long, User> u in _users) // loop through the dictionary of users
                {
                    if (u.Value.LastName.ToLower().Contains(search.ToLower())) // case insensitive search
                    {
                        results.Add(u.Key, u.Value); // add if found
                    }
                }
            }
            usersList.ItemsSource = results;
        }
        /// <summary>
        /// Shows all users currently stored in the data layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            searchUsersInput.Text = "";
            usersList.ItemsSource = _users;
        }

        /// <summary>
        /// Save the user as the current user and return to RecordEventWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usersList.SelectedItems.Count != 0)
            {
                User newUser = (User)((KeyValuePair<long,User>)usersList.SelectedItem).Value; // get the user
                DataFacade.SetDefaultUser(newUser); // save
                MessageBox.Show("New current user: " + newUser.ToString());
                ReturnBtn_Click(sender, e); // return to RecordEvents
            }
            else
            {
                MessageBox.Show("Please select a new user from the list.");
            }
        }

        /// <summary>
        /// Return to RecordEventsWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            _recordEventWindow = new RecordEventsWindow();
            _recordEventWindow.Show();
            this.Close();
        }
    }
}
