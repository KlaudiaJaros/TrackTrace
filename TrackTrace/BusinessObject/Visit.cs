using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace TrackTrace.BusinessObject
{
    /// <summary>
    /// Stores information about a visit: the User involved and the Location. It inherits from the Event class. 
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    class Visit : Event
    {
        public User User { get; set; }
        public Location Location { get; set; } 
        public override string ToString()
        {
            return "Visit: " + base.ID.ToString() + ", date: " + base.DateAndTime.ToString() + ", user: " + User.ToString() + ", location: " + Location.ID + ", " + Location.Name;
        }
        /// <summary>
        /// Converts Visit object into a comma separated values string to make it easier to save the stored properties in a CSV file.
        /// </summary>
        /// <returns>CSV string of all properites stored.</returns>
        public override string ToCSV()
        {
            return base.ToCSV() + "," + User.ToCSV() + "," + Location.ToCSV();
        }
    }
}
