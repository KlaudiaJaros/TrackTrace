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

namespace TrackTrace.Presentation
{
    /// <summary>
    /// Interaction logic for RecordEventsWindow.xaml
    /// </summary>
    public partial class RecordEventsWindow : Window
    {
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
            
        }

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

        }

        private void SearchVisitContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (recordContactsBtn.IsChecked==false && recordVisitsBtn.IsChecked == false)
            {
                warningText.Visibility = Visibility.Visible;
            }
            else if (recordContactsBtn.IsChecked==true && recordVisitsBtn.IsChecked==false)
            {
                SearchUsers();
                warningText.Visibility = Visibility.Hidden;
            }
            else if(recordVisitsBtn.IsChecked==true && recordContactsBtn.IsChecked == false)
            {
                SearchLocations();
                warningText.Visibility = Visibility.Hidden;
            }
                
        }
        public void SearchUsers()
        {

        }

        public void SearchLocations()
        {

        }
        public void RecordVisits()
        {

        }

        public void RecordContacts()
        {

        }



        private void HelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Visit - occurs when a user checks in at a particular location. To record a visit, select 'Visits' and use the tool below to find a location which was visited by the user.\n" +
                "Contact - occurs when two users have come into contact. To record a contact, select 'Contacts' and use the tool below to find a person the user has been in contact with.";
            HelpImg.ToolTip = tp;
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void SaveExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: save event 

            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow window = new AddUserWindow();
            window.Show();
            this.Close();
        }

        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLocationWindow window = new AddLocationWindow();
            window.Show();
            this.Close();
        }

        private void SearchUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserLNSearchBtn.IsChecked==false && UserIDSearchBtn.IsChecked == false)
            {
                MessageBox.Show("Please select if you are searching by User's last name or ID.");
            }
            else if (UserLNSearchBtn.IsChecked==true && UserIDSearchBtn.IsChecked==false)
            {
                // TODO: search by last name
            }
            else if(UserIDSearchBtn.IsChecked==true && UserLNSearchBtn.IsChecked == false)
            {
                // TODO: search by user id
            }
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
    }
}
