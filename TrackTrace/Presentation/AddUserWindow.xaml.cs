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
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                //  TODO: save user
                User newUser = new User();
                newUser.SetFirstName(firstNameBox.Text);
                newUser.SetLastName(lastNameBox.Text);
                newUser.SetPhoneNo(phoneNoBox.Text);


                TextConnectorFacade.SaveUser(newUser);
                MessageBox.Show("User(s) successfully added.");
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
        }

        private void AddAnotherUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                //  TODO: save user
                firstNameBox.Text = "";
                lastNameBox.Text = "";
                phoneNoBox.Text = "";
            }

        }

        // TODO: add extra cases: max input
        /// <summary>
        /// A method that takes all user's input from the form and checks if it is valid.
        /// </summary>
        /// <returns> Boolean: true, if the input is valid, false if not.</returns>
        private bool ValidateInput()
        {
            bool isValid = true;

            // check name:
            if(firstNameBox.Text=="" || firstNameBox.Text.Length>70)
            {
                isValid = false;
                MessageBox.Show("Please provide a valid first name.");
            }
            if (lastNameBox.Text == "" || lastNameBox.Text.Length>70)
            {
                isValid = false;
                MessageBox.Show("Please provide a valid last name.");
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

        private void fnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "User's first name. Max 70 characters.";
            fnImg.ToolTip = tp;
        }

        private void pnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "User's phone number. UK mobile format: 07XXXXXXXXX or UK landline format.";
            pnImg.ToolTip = tp;
        }

        private void lnImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "User's last name. Max 70 characters.";
            lnImg.ToolTip = tp;
        }
    }
}
