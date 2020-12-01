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
        // to store loaded data for the purpose of this window:
        private List<User> users = new List<User>();
        private List<Location> locations = new List<Location>();
        private MainWindow mainMenu;
        private AddUserWindow addUserWindow;
        private AddLocationWindow addLocationWindow;

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

            // ensure the user can only select one result at a time:
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
            locations = DataFacade.GetLocations();
            users = DataFacade.GetUsers();
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
        /// Event handler for 'Save Event and Exit' button. The function saves the Event and returns to MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: save event 
            if (ValidateInput())
            {
                User selectedUser = (User)usersList.SelectedItem;
                if (recordContactsBtn.IsChecked == true)
                {
                    User contactUser = (User)resultsList.SelectedItem;
                    if (selectedUser.GetId() == contactUser.GetId())
                    {
                        MessageBox.Show("Please select two different users.");
                    }
                    else
                    {
                        Contact saveContact = new Contact();
                        saveContact.SetDateTime(DateTimePickCtr.Value);
                        saveContact.User1 = selectedUser;
                        saveContact.User2 = contactUser;

                        DataFacade.SaveEvent(saveContact);
                        MessageBox.Show("Contact for " + selectedUser.ToString() + " successfully saved.");
                    }
                }
                else if (recordVisitsBtn.IsChecked == true)
                {
                    Location visitLocation = (Location)resultsList.SelectedItem;
                    Visit saveVisit = new Visit();
                    saveVisit.SetDateTime(DateTimePickCtr.Value);
                    saveVisit.User = selectedUser;
                    saveVisit.Location = visitLocation;

                    DataFacade.SaveEvent(saveVisit);
                    MessageBox.Show("Visit for " + selectedUser.ToString() + " successfully saved.");
                }
                

                // if the save and exit button called this method, exit:
                Button b = (Button)sender;
                if (b.Name == "SaveExitBtn")
                {
                    mainMenu = new MainWindow();
                    mainMenu.Show();
                    this.Close();
                }
            }
        }
        /// <summary>
        /// Event handler for 'Save Event and Add another' button. The function saves the event and clears the form so the user can add another event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAnotherBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveExitBtn_Click(sender, e);
            DateTimePickCtr.Value=DateTime.Now;
            usersList.SelectedItem = null;
            resultsList.SelectedItem = null;
            selectedItemDisplay.Text = "";
            selectedUserDisplay.Text = "";
            userInput.Text = "";
            userInput2.Text = "";

        }

        /// <summary>
        /// Searches through the list of users and returns all that match the user input string.
        /// </summary>
        /// <param name="byID">Boolean to deduct if the method should search by user's id or not. If not, the default is to search by user's last name.</param>
        /// <param name="userInput">User's input.</param>
        /// <returns>A list off all users that match the provided string.</returns>
        public List<User> SearchUsers(bool byID, string userInput)
        {
            string search = userInput;
            List<User> results = new List<User>();

            if (byID)
            {
                foreach (User u in users)
                {
                    int id = 0;
                    Int32.TryParse(search, out id);
                    if (u.GetId() == id)
                    {
                        results.Add(u);
                    }
                }
            }
            else 
            {
                foreach (User u in users)
                {
                    if (u.GetLastName().ToLower().Contains(search.ToLower()))
                    {
                        results.Add(u);
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
                foreach(Location l in locations)
                {
                    if (l.PostCode.ToLower().Equals(userInput.ToLower()))
                    {
                        results.Add(l);
                    }
                }
            }
            else
            {
                foreach(Location l in locations)
                {
                    if (l.Name.ToLower().Contains(userInput.ToLower()))
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
            addUserWindow = new AddUserWindow();
            addUserWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Closes the current window and opens the AddLocation Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            addLocationWindow = new AddLocationWindow();
            addLocationWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Search button event handler for the first listbox. The user can search for users.
        /// </summary>
        /// <param name="sender"></param>
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
            usersList.ItemsSource = users;
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
                resultsList.ItemsSource = users;
            }
            else if (recordVisitsBtn.IsChecked == true && recordContactsBtn.IsChecked == false)
            {
                // show locations:
                warningText.Visibility = Visibility.Hidden;
                resultsList.ItemsSource = locations;           
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
            mainMenu = new MainWindow();
            mainMenu.Show();
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

        private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersList.SelectedItem != null)
            {
                selectedUserDisplay.Text = (usersList.SelectedItem).ToString();
            }           
        }

        private void resultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsList.SelectedItem != null)
            {
                selectedItemDisplay.Text = (resultsList.SelectedItem).ToString();
            }       
        }
    }
}
