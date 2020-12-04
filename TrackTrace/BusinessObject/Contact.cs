using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    /// <summary>
    /// Stores information about a contact: two Users involved. It inherits from the Event class.
    /// Created by: Klaudia Jaros   
    /// Last modified: 04/12/2020
    /// </summary>
    class Contact : Event
    {
        public User User1 { get; set; }
        public User User2 { get; set; }

        public override string ToString()
        {
            return "Contact: " + base.ID.ToString() + ", date: " + base.DateAndTime.ToString() +  ", users: " + User1.ToString() + ", " + User2.ToString();
        }

        /// <summary>
        /// Converts a Contact object into a comma separated values string to make it easier to save the stored properties in a CSV file.
        /// </summary>
        /// <returns>CSV string of all properites stored.</returns>
        public override string ToCSV()
        {
            return base.ToCSV() + "," + User1.ToCSV() +"," + User2.ToCSV();
        }
    }
}
