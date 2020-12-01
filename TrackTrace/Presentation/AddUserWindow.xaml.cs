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
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private MainWindow mainMenu;
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                User newUser = new User();
                newUser.SetFirstName(firstNameBox.Text);
                newUser.SetLastName(lastNameBox.Text);
                newUser.SetPhoneNo(phoneNoBox.Text);
                DataFacade.SaveUser(newUser);

                MessageBox.Show("User successfully added with a user id: " + newUser.GetId() + ". \nPlease save it for future reference.");
                mainMenu = new MainWindow();
                mainMenu.Show();
                this.Close();
            }
        }

        private void AddAnotherUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                User newUser = new User();
                newUser.SetFirstName(firstNameBox.Text);
                newUser.SetLastName(lastNameBox.Text);
                newUser.SetPhoneNo(phoneNoBox.Text);
                DataFacade.SaveUser(newUser);

                firstNameBox.Text = "";
                lastNameBox.Text = "";
                phoneNoBox.Text = "";
                MessageBox.Show("User successfully added with a user id: " + newUser.GetId() + ". \nPlease save it for future reference.");
            }

        }

        /// <summary>
        /// A method that takes all user's input from the form and checks if it is valid.
        /// </summary>
        /// <returns> Boolean: true, if the input is valid, false if not.</returns>
        private bool ValidateInput()
        {
            bool isValid = true;

            // check name:
            if(firstNameBox.Text.Length>70)
            {
                isValid = false;
                MessageBox.Show("Please provide a valid first name or leave the field empty.");
            }
            if (lastNameBox.Text.Length>70)
            {
                isValid = false;
                MessageBox.Show("Please provide a valid last name or leave the field empty.");
            }

            // check phone number:
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


            return isValid;
        }

        // Help tips for the user:
        private void fnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Optional field. User's first name. Max 70 characters.";
            fnImg.ToolTip = tp;
        }

        private void pnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Mandatory field. User's phone number. UK mobile format: 07XXXXXXXXX or UK landline format.";
            pnImg.ToolTip = tp;
        }

        private void lnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Optional field. User's last name. Max 70 characters.";
            lnImg.ToolTip = tp;
        }
    }
}
