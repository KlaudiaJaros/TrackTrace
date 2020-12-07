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
    /// Interaction logic for RecordEventsWindow.xaml. Displays an interactive form to record events for users both contacts and visits. It allows the
    /// user to search for users and locations logged in the system in order to record an event. 
    /// Created by: Klaudia Jaros   
    /// Last modified: 04/12/2020
    /// </summary>
    public partial class RecordEventsWindow : Window
    {
        // to store loaded data for the purpose of this window:
        private Dictionary<long,User> _users = new Dictionary<long, User>();
        private List<Location> _locations = new List<Location>();

        // navigation:
        private MainWindow _mainMenu;
        private AddUserWindow _addUserWindow;
        private AddLocationWindow _addLocationWindow;

        public RecordEventsWindow()
        {
            InitializeComponent();

            // group radio buttons so they do not interfere with each other:
            GroupBox visitsAndContacts = new GroupBox();
            visitsAndContacts.Content = recordVisitsBtn;
            visitsAndContacts.Content = recordContactsBtn;
            
            GroupBox userNameID = new GroupBox();
            userNameID.Content = UserLNSearchBtn;
            userNameID.Content = UserIDSearchBtn;

            GroupBox locationContacts = new GroupBox();
            locationContacts.Content = ContactNameSearchBtn;
            locationContacts.Content = ContactIDSearchBtn;
            locationContacts.Content = LocNameSearchBtn;
            locationContacts.Content = PostCodeSearchBtn;

            // ensure the user can only select one User or one User/Location from the lists at a time:
            usersList.SelectionMode = SelectionMode.Single;
            resultsList.SelectionMode = SelectionMode.Single;

            // set the date to now:
            DateTimePickCtr.Value = DateTime.Now;
            DateTimePickCtr.Maximum = DateTime.Now;
            DateTimePickCtr.ClipValueToMinMax = true;

            LoadData();
        }
        /// <summary>
        /// Loads data for users and locations using DataFacade.
        /// </summary>
        private void LoadData()
        {
            _locations = DataFacade.GetLocations();
            _users = DataFacade.GetUsers();
        }
        /// <summary>
        /// Checks if the user provided all the information needed to save an Event. If not, it shows a MessageBox informing what's wrong.
        /// </summary>
        /// <returns>True, if the user input is valid, otherwise false.</returns>
        private bool ValidateInput()
        {
            bool valid = true;
            if (usersList.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have to select a user (from the list on the left) for who you would like to record an event.");
                valid = false;
            }                
            if (resultsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have to select a user (if recording a contact) or a location (if recording a visit) from the list on the right.");
                valid = false;
            }            
            if (DateTimePickCtr.Value == null)
            {
                MessageBox.Show("Please provide a time and a date for the event.");
                valid = false;
            }
            return valid;    
        }
        /// <summary>
        /// Event handler for 'Save Event and Exit' or "Save and Add another" button. The function saves the Event and either returns to MainWindow or clear the form for the next record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                User selectedUser = (User)((KeyValuePair<long, User>)usersList.SelectedItem).Value; // user selection is a KeyValuePair, get the Value and cast it into a User
                if (recordContactsBtn.IsChecked == true)
                {
                    User contactUser = (User)((KeyValuePair<long,User>)resultsList.SelectedItem).Value;
                    if (selectedUser.ID == contactUser.ID)
                    {
                        MessageBox.Show("Please select two different users.");
                    }
                    else
                    {
                        Contact saveContact = new Contact();
                        saveContact.DateAndTime = (DateTime)DateTimePickCtr.Value;
                        saveContact.User1 = selectedUser;
                        saveContact.User2 = contactUser;

                        DataFacade.SaveEvent(saveContact);
                        MessageBox.Show("Contact for " + selectedUser.ToString() + " successfully saved.");
                    }
                }
                else if (recordVisitsBtn.IsChecked == true)
                {
                    Location visitLocation = (Location)resultsList.SelectedItem; // cast the selected item into a Location object
                    Visit saveVisit = new Visit();
                    saveVisit.DateAndTime = (DateTime)DateTimePickCtr.Value;
                    saveVisit.User = selectedUser;
                    saveVisit.Location = visitLocation;

                    DataFacade.SaveEvent(saveVisit);
                    MessageBox.Show("Visit for " + selectedUser.ToString() + " successfully saved.");
                }
                

                // if the save and exit button called this method, exit:
                Button b = (Button)sender;
                if (b.Name == "SaveExitBtn")
                {
                    _mainMenu = new MainWindow();
                    _mainMenu.Show();
                    this.Close();
                }

                // if not, clear the form:
                DateTimePickCtr.Value = DateTime.Now;
                usersList.SelectedItem = null;
                resultsList.SelectedItem = null;
                selectedItemDisplay.Text = "";
                selectedUserDisplay.Text = "";
                userInput.Text = "";
                userInput2.Text = "";
            }
        }

        /// <summary>
        /// Searches through the list of users and returns all that match the user input string.
        /// </summary>
        /// <param name="byID">Boolean to deduct if the method should search by user's id or not. If not, the default is to search by user's last name.</param>
        /// <param name="userInput">User's input. Either a number (if id search) or a last name.</param>
        /// <returns>A list off all users that match the given string.</returns>
        public Dictionary<long, User> SearchUsers(bool byID, string userInput)
        {
            string search = userInput;
            Dictionary<long,User> results = new Dictionary<long, User>();

            if (byID)
            {
                long id = 0;
                long.TryParse(search, out id); // parse id to long
                _users.TryGetValue(id, out User u); // check if it's in the dictionary
                results.Add(id, u);
            }
            else 
            {               
                foreach (KeyValuePair<long,User> u in _users) // loop through the dictionary of users
                {
                    if (u.Value.LastName.ToLower().Contains(search.ToLower())) // case insensitive search
                    {
                        results.Add(u.Key,u.Value); // Add(u);
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
                foreach(Location l in _locations)
                {
                    if (l.PostCode.ToLower().Equals(userInput.ToLower())) // case insensitive search
                    {
                        results.Add(l);
                    }
                }
            }
            else
            {
                foreach(Location l in _locations)
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
        /// Search button event handler for the first listbox. The user can search for users either by ID or last name by checking the right radio button.
        /// </summary>
        /// <param name="sender">'Search' button</param>
        /// <param name="e"></param>
        private void SearchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedUserDisplay.Text = "";

            if (UserLNSearchBtn.IsChecked==false && UserIDSearchBtn.IsChecked == false)
            {
                MessageBox.Show("Please select if you are searching by User's last name or ID.");
            }
            else if (UserLNSearchBtn.IsChecked==true && UserIDSearchBtn.IsChecked==false)
            {
                usersList.ItemsSource= SearchUsers(false, userInput.Text);
            }
            else if(UserIDSearchBtn.IsChecked==true && UserLNSearchBtn.IsChecked == false)
            {
                usersList.ItemsSource=SearchUsers(true, userInput.Text);
            }
        }
        /// <summary>
        /// Search button event handler for the second listbox. The user can search for contacts or locations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchVisitContactBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedItemDisplay.Text = "";

            if (recordContactsBtn.IsChecked == false && recordVisitsBtn.IsChecked == false)
            {
                warningText.Visibility = Visibility.Visible;
            }
            else if (recordContactsBtn.IsChecked == true && recordVisitsBtn.IsChecked == false)
            {
                if (ContactIDSearchBtn.IsChecked == true)
                    resultsList.ItemsSource = SearchUsers(true, userInput2.Text);
                else if (ContactNameSearchBtn.IsChecked == true)
                    resultsList.ItemsSource = SearchUsers(false, userInput2.Text);

                warningText.Visibility = Visibility.Hidden;
            }
            else if (recordVisitsBtn.IsChecked == true && recordContactsBtn.IsChecked == false)
            {
                if (PostCodeSearchBtn.IsChecked == true)
                    resultsList.ItemsSource = SearchLocations(true, userInput2.Text);
                else if (LocNameSearchBtn.IsChecked == true)
                    resultsList.ItemsSource = SearchLocations(false, userInput2.Text);

                warningText.Visibility = Visibility.Hidden;
            }

        }
        /// <summary>
        /// Displays all users in the first listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            usersList.ItemsSource = _users;
        }

        /// <summary>
        /// Displays all locations or contacts in the second listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAllBtn_Click(object sender, RoutedEventArgs e)
        {
            userInput2.Text = "";
            if (recordContactsBtn.IsChecked == false && recordVisitsBtn.IsChecked == false)
            {
                warningText.Visibility = Visibility.Visible;
            }
            else if (recordContactsBtn.IsChecked == true && recordVisitsBtn.IsChecked == false)
            {
                // show users:
                warningText.Visibility = Visibility.Hidden;
                resultsList.ItemsSource = _users;
            }
            else if (recordVisitsBtn.IsChecked == true && recordContactsBtn.IsChecked == false)
            {
                // show locations:
                warningText.Visibility = Visibility.Hidden;
                resultsList.ItemsSource = _locations;           
            }
        }
        /// <summary>
        /// Event handler for the question mark image. Displays a helpul tip for the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Visit - occurs when a user checks in at a particular location.\n" +
                "Contact - occurs when two users have come into contact.";
            HelpImg.ToolTip = tp;
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

        /* RadioButtons management below: 
          - ensures that all of the RadioButtons work well
          - depending on which button is on, different forms/textblocks appear
         */
        private void recordVisitsBtn_Checked(object sender, RoutedEventArgs e)
        {
            recordContactsBtn.IsChecked = false;
            // hide contacts:
            ContactIDSearchBtn.Visibility = Visibility.Hidden;
            ContactNameSearchBtn.Visibility = Visibility.Hidden;
            contactText.Visibility = Visibility.Hidden;

            // show visits:
            visitText.Visibility = Visibility.Visible;
            LocNameSearchBtn.Visibility = Visibility.Visible;
            PostCodeSearchBtn.Visibility = Visibility.Visible;

            resultsList.ItemsSource = null; // clear the results list
        }

        private void recordContactsBtn_Checked(object sender, RoutedEventArgs e)
        {
            recordVisitsBtn.IsChecked = false;
            // hide visits:
            visitText.Visibility = Visibility.Hidden;
            LocNameSearchBtn.Visibility = Visibility.Hidden;
            PostCodeSearchBtn.Visibility = Visibility.Hidden;

            // show contacts:
            ContactIDSearchBtn.Visibility = Visibility.Visible;
            ContactNameSearchBtn.Visibility = Visibility.Visible;
            contactText.Visibility = Visibility.Visible;

            resultsList.ItemsSource = null; // clear the results list
        }

        /// <summary>
        /// Displays currently selected user for whom the event is about the be added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersList.SelectedItem != null)
            {
                selectedUserDisplay.Text = (usersList.SelectedItem).ToString();
            }           
        }

        /// <summary>
        /// Displays currently selected user (if recording contacts) or location (if recording visits)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsList.SelectedItem != null)
            {
                selectedItemDisplay.Text = (resultsList.SelectedItem).ToString();
            }       
        }

        // radio buttons event handlers to ensure that if one is checked, the other is unchecked:
        private void UserLNSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            UserIDSearchBtn.IsChecked = false;
        }

        private void UserIDSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            UserLNSearchBtn.IsChecked = false;
        }

        private void LocNameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            PostCodeSearchBtn.IsChecked = false;
        }

        private void PostCodeSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            LocNameSearchBtn.IsChecked = false;
        }

        private void ContactIDSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            ContactNameSearchBtn.IsChecked = false;
        }

        private void ContactNameSearchBtn_Checked(object sender, RoutedEventArgs e)
        {
            ContactIDSearchBtn.IsChecked = false;
        }
    }
}
