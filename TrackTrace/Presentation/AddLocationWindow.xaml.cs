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
    /// Interaction logic for AddLocationWindow.xaml
    /// </summary>
    public partial class AddLocationWindow : Window
    {
        public AddLocationWindow()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddLocBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                // TODO: save location
                MessageBox.Show("Location(s) successfully added.");
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
        }

        private void AddAnotherLocBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                // TODO: save location
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

            // 
            if (locNameBox.Text == "" || locNameBox.Text.Length>70)
            {
                isValid = false;
                MessageBox.Show("Please provide location's name. Max 70 characters.");
            }
            if (locAddressBox.Text == "" || locAddressBox.Text.Length>35 || locAddress2Box.Text.Length>35)
            {
                isValid = false;
                MessageBox.Show("Please provide location's address. Max 35 characters per line.");
            }
            if (locPostCodeBox.Text == "" || !(locPostCodeBox.Text.Length >=6 && locPostCodeBox.Text.Length<=8))
            {
                isValid = false;
                MessageBox.Show("Please provide a valid post code.");
            }
 
            if (locTownBox.Text == "" || locTownBox.Text.Length>35)
            {
                isValid = false;
                MessageBox.Show("Please provide location's town. Max 35 characters.");
            }


            return isValid;
        }

        private void NameImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's name. Max 70 characters.";
            NameImg.ToolTip = tp;
        }

        private void AddImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's address. Max 35 characters per line. Second line is optional.";
            AddImg.ToolTip = tp;
        }

        private void PcImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's post code. UK post code: between 6-8 characters.";
            PcImg.ToolTip = tp;
        }

        private void TownImg_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.Content = "Location's town/city. Max 35 characters.";
            TownImg.ToolTip = tp;
        }
    }
}
