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
    /// Interaction logic for AddUserWindow.xaml. It displays a form to add a new user and after validating the input it adds a new user to the system.
    /// Created by: Klaudia Jaros   
    /// Last modified: 09/12/2020
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private MainWindow _mainMenu;
        public AddUserWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns to Main Window.
        /// </summary>
        /// <param name="sender">'Return' Button.</param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenu = new MainWindow();
            _mainMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Event handler for Save buttons. If ValidateInput returns true, the new user is added. 
        /// </summary>
        /// <param name="sender">Either 'Save and Add Another' button or 'Save and Exit' button.</param>
        /// <param name="e"></param>
        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                User newUser = new User();
                newUser.FirstName=firstNameBox.Text;
                newUser.LastName=lastNameBox.Text;
                newUser.PhoneNumber=phoneNoBox.Text;
                DataFacade.SaveUser(newUser);

                MessageBox.Show("User successfully added with a user id: " + newUser.ID + ". \nPlease save it for future reference.");

                if (yesBtn.IsChecked == true) // set the user as default if requested by the user
                {
                    DataFacade.SetDefaultUser(newUser);
                }

                // check if the 'Save and Exit' button called this method, if yes, exit:
                Button btn = (Button)sender;
                if (btn.Name.Equals("AddUserExitBtn"))
                {
                    _mainMenu = new MainWindow();
                    _mainMenu.Show();
                    this.Close();
                }

                // clear TextBoxes for adding another user:
                firstNameBox.Text = "";
                lastNameBox.Text = "";
                phoneNoBox.Text = "";
            }
        }


        /// <summary>
        /// A method that takes all user's input from the form and checks if it is valid.
        /// </summary>
        /// <returns> Boolean: true, if the input is valid, false if not.</returns>
        private bool ValidateInput()
        {
            bool isValid = true;

            // check name < 70 characters and contains no commas:
            if(firstNameBox.Text.Length>70 || firstNameBox.Text.Contains(","))
            {
                isValid = false;
                MessageBox.Show("Please provide a valid first name or leave the field empty.");
            }
            if (lastNameBox.Text.Length > 70 || firstNameBox.Text.Contains(","))
            {
                isValid = false;
                MessageBox.Show("Please provide a valid last name or leave the field empty.");
            }
            // check phone number if numeric and the correct length:
            bool isNumeric = true;
            foreach(char c in phoneNoBox.Text)
            {
                if (!Char.IsNumber(c))
                {
                    isNumeric = false;
                }
            }
            // 10 or 11 for landline, 11 for 07.. mobile:
            if (!isNumeric || !(phoneNoBox.Text.Length==11 || phoneNoBox.Text.Length==10) )
            {
                isValid = false;
                MessageBox.Show("Please provide a valid phone number.");
            }
            else if (noBtn.IsChecked == false && yesBtn.IsChecked == false)
            {
                MessageBox.Show("Please indicate if the user you are adding should be set as the default user.");
                isValid = false;
            }

            return isValid;
        }

        // Help tips for the user when they hoover over the help image:
        private void FirstNameHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Optional field. User's first name. Max 70 characters.";
            fnImg.ToolTip = tp;
        }

        private void PhoneNoHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Mandatory field. User's phone number. UK mobile format: 07XXXXXXXXX or UK landline format.";
            pnImg.ToolTip = tp;
        }

        private void LastNameHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Optional field. User's last name. Max 70 characters.";
            lnImg.ToolTip = tp;
        }

        private void defaultImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Default user to save events for.";
            defaultImg.ToolTip = tp;
        }
    }
}
