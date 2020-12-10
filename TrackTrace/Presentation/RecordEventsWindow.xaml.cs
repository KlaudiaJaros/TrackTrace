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
    /// Interaction logic for RecordEventsWindow.xaml
    /// </summary>
    public partial class RecordEventsWindow : Window
    {
        // collections to store loaded data for the purpose of this window:
        private Dictionary<long, User> _users = new Dictionary<long, User>();
        private List<Location> _locations = new List<Location>();

        // window navigation:
        private MainWindow _mainMenu;
        private AddUserWindow _addUserWindow;
        private AddLocationWindow _addLocationWindow;
        private SwitchUsersWindow _switchUsersWindow;

        public RecordEventsWindow()
        {
            InitializeComponent();

            // group radio buttons so they do not interfere with each other:

            GroupBox contactsIdorLastName = new GroupBox();
            contactsIdorLastName.Content = ContactIDSearchBtn;
            contactsIdorLastName.Content = ContactLNSearchBtn;

            GroupBox locationBtns = new GroupBox();
            locationBtns.Content = LocNameSearchBtn;
            locationBtns.Content = PostCodeSearchBtn;

            // ensure the user can only select one User or one User/Location from the lists at a time:
            resultsList.SelectionMode = SelectionMode.Single;
            contactUsersList.SelectionMode = SelectionMode.Single;

            // set the date to now:
            DateTimePickCtr.Value = DateTime.Now;
            DateTimePickCtr.Maximum = DateTime.Now;
            DateTimePickCtr.ClipValueToMinMax = true;

            // load data from the data layer:
            LoadData();
        }
        /// <summary>
        /// Loads data for users and locations using DataFacade.
        /// </summary>
        private void LoadData()
        {
            _locations = DataFacade.GetLocations();
            _users = DataFacade.GetUsers();

            User defaultUser = DataFacade.GetDefaultUser();
            if (defaultUser != null) // set the default user if there is one
            {
                selectedUser.Text = defaultUser.ToString();
            }
        }
        /// <summary>
        /// Checks if the user provided all the information needed to save an Event. If not, it shows a MessageBox informing what's wrong.
        /// </summary>
        /// <returns>True, if the user input is valid, otherwise false.</returns>
        private bool ValidateInput()
        {
            bool valid = true;

            if (DateTimePickCtr.Value == null)
            {
                MessageBox.Show("Please provide a time and a date for the event.");
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Saves a contact for the selected/default user.
        /// </summary>
        /// <param name="sender">'Save contact' button.</param>
        /// <param name="e"></param>
        private void SaveContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput() && contactUsersList.SelectedItems.Count != 0)
            {
                User user;

                user = DataFacade.GetDefaultUser();

                User contactUser = (User)((KeyValuePair<long, User>)contactUsersList.SelectedItem).Value; // get the contact user
                if (user.ID == contactUser.ID)
                {
                    MessageBox.Show("Please select two different users.");
                }
                else
                {
                    // create a new contact and save it:
                    Contact saveContact = new Contact();
                    saveContact.DateAndTime = (DateTime)DateTimePickCtr.Value;
                    saveContact.User1 = user;
                    saveContact.User2 = contactUser;

                    // Configure the message box to be displayed:
                    string messageBoxText = "Do you want to save Contact:\n" + user.ToString() + " and\n" + contactUser.ToString() + "\non " + saveContact.DateAndTime + "?";
                    string caption = "Record contact requires confirmation";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Question;

                    // Display a message box:
                    MessageBoxResult result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);

                    if (result == MessageBoxResult.Yes) // if user answers yes:
                    {
                        DataFacade.SaveEvent(saveContact);
                        MessageBox.Show("Contact for " + selectedUser.Text + " successfully saved.");
                    }
                }
            }
            else
            {
                MessageBox.Show("You have to select a user you/selected user were in contact with.");
            }
        }

        /// <summary>
        /// Saves the visit for the selected user or both users if recording a contact with a location.
        /// </summary>
        /// <param name="sender">'Save visit' button</param>
        /// <param name="e"></param>
        private void SaveVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput() && resultsList.SelectedItems.Count != 0)
            {
                User user;
                user = DataFacade.GetDefaultUser(); // retrieve default user from data layer

                // get the location and create a visit:
                Location visitLocation = (Location)resultsList.SelectedItem; // cast the selected item into a Location object
                Visit saveVisit = new Visit(); // create a visit
                saveVisit.DateAndTime = (DateTime)DateTimePickCtr.Value;
                saveVisit.User = user;
                saveVisit.Location = visitLocation;

                if (contactUsersList.SelectedItems.Count != 0) // if there is a user 2 (in case of a contact with a location):
                {
                    // get the second user and create a visit for them:
                    User contactUser = (User)((KeyValuePair<long, User>)contactUsersList.SelectedItem).Value; // KeyValuePair, get the Value and cast it into a User                    
                    Visit saveVisit2 = new Visit(); // create a visit for user 2
                    saveVisit2.DateAndTime = (DateTime)DateTimePickCtr.Value;
                    saveVisit2.User = contactUser;
                    saveVisit2.Location = visitLocation;

                    // Configure the message box to be displayed:
                    string messageBoxText = "Do you want to save Visit:\n" + user.ToString() + "\nand " + contactUser.ToString()
                        + "\nto " + visitLocation.Name + " " + visitLocation.Town + "\non " + saveVisit.DateAndTime + "?";
                    string caption = "Record visit requires confirmation";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Question;

                    // Display a warning message box:
                    MessageBoxResult result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);

                    if (result == MessageBoxResult.Yes) // if user answer yes:
                    {
                        DataFacade.SaveEvent(saveVisit); // save visit for user 1
                        DataFacade.SaveEvent(saveVisit2); // save visit for user 2
                        MessageBox.Show("Visit for " + selectedUser.Text + " and " + contactUser.ToString() + " successfully saved.");
                    }
                }
                else // save visit for one user:
                {
                    // Configure the message box to be displayed:
                    string messageBoxText = "Do you want to save Visit:\n" + user.ToString() + " to\n" + visitLocation.Name + " " + visitLocation.Town
                        + "\non " + saveVisit.DateAndTime + "?";
                    string caption = "Record visit requires confirmation";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Question;

                    // Display a warning message box:
                    MessageBoxResult result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);

                    if (result == MessageBoxResult.Yes) // if user answer yes:
                    {
                        DataFacade.SaveEvent(saveVisit); // save visit for user 1
                        MessageBox.Show("Visit for " + selectedUser.Text + " successfully saved.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a location in order to record a visit.");
            }
        }
        /// <summary>
        /// Clears the entire form.
        /// </summary>
        /// <param name="sender">'Clear' button</param>
        /// <param name="e"></param>
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTimePickCtr.Value = DateTime.Now;
            resultsList.SelectedItem = null;
            contactUsersList.SelectedItem = null;
            contactUsersList.ItemsSource = null;
            resultsList.ItemsSource = null;
            selectedItemDisplay.Text = "";
            selectedUserDisplay.Text = "";
            contactInput.Text = "";
            locationInput.Text = "";
        }

        /// <summary>
        /// Searches through the list of users and returns all that match the user input string.
        /// </summary>
        /// <param name="byID">Boolean to deduct if the method should search by user's id or not. If not, the default is to search by user's last name.</param>
        /// <param name="userInput">User's input. Either a number (if id search) or a last name.</param>
        /// <returns>A dictionary with all users that match the given input.</returns>
        public Dictionary<long, User> SearchUsers(bool byID, string userInput)
        {
            string search = userInput;
            Dictionary<long, User> results = new Dictionary<long, User>();

            if (byID)
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
            return results;
        }
        /// <summary>
        /// Searched through the list of locations and returns all that match the user input string.
        /// </summary>
        /// <param name="byPostCode">Boolean to deduct if the method should search by locations's post-code or not. If not, the default is to search by locations's name.</param>
        /// <param name="userInput"></param>
        /// <returns>A list off all locations that match the provided string.</returns>
        public List<Location> SearchLocations(bool byPostCode, string userInput)
        {
            List<Location> results = new List<Location>();

            if (byPostCode)
            {
                foreach (Location l in _locations)
                {
                    if (l.PostCode.ToLower().Equals(userInput.ToLower())) // case insensitive search
                    {
                        results.Add(l);
                    }
                }
            }
            else
            {
                foreach (Location l in _locations)
                {
                    if (l.Name.ToLower().Contains(userInput.ToLower())) // case insensitive search
                    {
                        results.Add(l);
                    }
                }
            }
            return results;
        }
        /// <summary>
        /// Closes the current window and opens the AddUser Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            _addUserWindow = new AddUserWindow();
            _addUserWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Closes the current window and opens the AddLocation Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            _addLocationWindow = new AddLocationWindow();
            _addLocationWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Search button event handler for the second listbox. The user can search for users either by ID or last name by checking the right radio button.
        /// </summary>
        /// <param name="sender">'Search' button for contatcs</param>
        /// <param name="e"></param>
        private void SearchContactsBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedUserDisplay.Text = "";

            if (ContactLNSearchBtn.IsChecked == false && ContactIDSearchBtn.IsChecked == false)
            {
                MessageBox.Show("Please select if you are searching by User's last name or ID.");
            }
            else if (ContactLNSearchBtn.IsChecked == true && ContactIDSearchBtn.IsChecked == false)
            {
                contactUsersList.ItemsSource = SearchUsers(false, contactInput.Text);
            }
            else if (ContactIDSearchBtn.IsChecked == true && ContactLNSearchBtn.IsChecked == false)
            {
                contactUsersList.ItemsSource = SearchUsers(true, contactInput.Text);
            }
        }
        /// <summary>
        /// Search button event handler for the third listbox. The user can search for locations by id or name to save visits.
        /// </summary>
        /// <param name="sender">'Seacrh' button</param>
        /// <param name="e"></param>
        private void SearchVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedItemDisplay.Text = "";

            if (PostCodeSearchBtn.IsChecked == true)
                resultsList.ItemsSource = SearchLocations(true, locationInput.Text);
            else if (LocNameSearchBtn.IsChecked == true)
                resultsList.ItemsSource = SearchLocations(false, locationInput.Text);
            else
                MessageBox.Show("Please select if you are searching by post-code or name.");

        }
        /// <summary>
        /// Displays all users in the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllUsersBtn_Click(object sender, RoutedEventArgs e)
        {
                contactInput.Text = "";
                contactUsersList.ItemsSource = _users;

        }

        /// <summary>
        /// Displays all locations or contacts in the second listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllBtn_Click(object sender, RoutedEventArgs e)
        {
            locationInput.Text = "";
            // show locations:
            resultsList.ItemsSource = _locations;

        }
        /// <summary>
        /// Returns to MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenu = new MainWindow();
            _mainMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Displays currently selected contact user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactUsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactUsersList.SelectedItem != null)
            {
                selectedUserDisplay.Text = (contactUsersList.SelectedItem).ToString();
            }
        }

        /// <summary>
        /// Displays currently selected location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsList.SelectedItem != null)
            {
                selectedItemDisplay.Text = (resultsList.SelectedItem).ToString();
            }
        }
        /// <summary>
        /// Closes this window and opens SwitchUserWindow
        /// </summary>
        /// <param name="sender">'Switch users' button</param>
        /// <param name="e"></param>
        private void switchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            _switchUsersWindow = new SwitchUsersWindow();
            _switchUsersWindow.Show();
            this.Close();
        }
        // radio buttons event handlers to ensure that if one is checked, the other is unchecked:

        private void LocNameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            PostCodeSearchBtn.IsChecked = false;
        }

        private void PostCodeSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            LocNameSearchBtn.IsChecked = false;
        }

        private void ContactLNSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            ContactIDSearchBtn.IsChecked = false;
        }

        private void ContactIDSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            ContactLNSearchBtn.IsChecked = false;
        }

    }
}
