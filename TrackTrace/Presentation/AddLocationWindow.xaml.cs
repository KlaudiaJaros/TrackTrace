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
    /// Interaction logic for AddLocationWindow.xaml. It diplays a form to add a new location and after input validation adds a new location to the system.
    /// Created by: Klaudia Jaros   
    /// Last modified: 09/12/2020
    /// </summary>
    public partial class AddLocationWindow : Window
    {
        private MainWindow _mainMenu;
        public AddLocationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns to Main Window.
        /// </summary>
        /// <param name="sender">'Return' button.</param>
        /// <param name="e"></param>
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenu = new MainWindow();
            _mainMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Event handler for Save Location buttons. If ValidateInput returns true, the new location is added.
        /// </summary>
        /// <param name="sender">Either 'Save and Exit' or 'Save and Add Another' button.</param>
        /// <param name="e"></param>
        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                Location loc = new Location();
                loc.Name = locNameBox.Text;
                loc.Address = locAddressBox.Text;
                loc.PostCode = locPostCodeBox.Text;
                loc.Town = locTownBox.Text;
                DataFacade.SaveLocation(loc);

                MessageBox.Show("Location(s) successfully added.");

                // check if 'Save and Exit' button called this method, if yes, exit:
                Button btn = (Button)sender;
                if (btn.Name.Equals("AddLocExitBtn"))
                {
                    _mainMenu = new MainWindow();
                    _mainMenu.Show();
                    this.Close();
                }

                // clear TextBoxes for adding a new location:
                locNameBox.Text = "";
                locAddressBox.Text = "";
                locPostCodeBox.Text = "";
                locTownBox.Text = "";
            }
        }

        
        /// <summary>
        /// A method that takes all user's input from the form and checks if it is valid.
        /// </summary>
        /// <returns> Boolean: true, if the input is valid, false if not.</returns>
        private bool ValidateInput()
        {
            bool isValid = true;

            // check each field for the correct length, no commas and if empty:
            if (locNameBox.Text == "" || locNameBox.Text.Length>70 || locNameBox.Text.Contains(","))
            {
                isValid = false;
                MessageBox.Show("Please provide location's name. Max 70 characters.");
            }
            if (locAddressBox.Text == "" || locAddressBox.Text.Length > 70 || locAddressBox.Text.Contains(","))
            {
                isValid = false;
                MessageBox.Show("Please provide location's address. Max 70 characters.");
            }
            if (locPostCodeBox.Text == "" || locPostCodeBox.Text.Contains(",") || !(locPostCodeBox.Text.Length >=6 && locPostCodeBox.Text.Length<=8))
            {
                isValid = false;
                MessageBox.Show("Please provide a valid post code.");
            }
 
            if (locTownBox.Text == "" || locTownBox.Text.Length>35 || locTownBox.Text.Contains(","))
            {
                isValid = false;
                MessageBox.Show("Please provide location's town. Max 35 characters.");
            }


            return isValid;
        }

        // help hints for the user when they hoover over a help image:
        private void NameHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's name. Max 70 characters.";
            NameImg.ToolTip = tp;
        }

        private void AddressHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's address. Max 70 characters, commas are not allowed.";
            AddImg.ToolTip = tp;
        }

        private void PostCodeHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's post code. UK post code: between 6-8 characters.";
            PcImg.ToolTip = tp;
        }

        private void TownHelpImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's town/city. Max 35 characters.";
            TownImg.ToolTip = tp;
        }
    }
}
