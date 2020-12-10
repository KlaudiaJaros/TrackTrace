using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    /// <summary>
    /// Stores information about a location: id, name, address, post-code and town.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    public class Location
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }

        public override string ToString()
        {
            return "Location: " + ID + ", name: " + Name + ", address: " + Address + ", " + PostCode + ", " + Town;
        }

        /// <summary>
        /// Converts Location object into a comma separated values string to make it easier to save the stored properties in a CSV file.
        /// </summary>
        /// <returns>CSV string of all properites stored.</returns>
        public string ToCSV()
        {
            return ID + "," + Name + "," + Address + "," + PostCode + "," + Town;
        }
    }
}
