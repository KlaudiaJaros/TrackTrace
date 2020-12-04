using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    /// <summary>
    /// Stores information about an event: id and datetime.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    public class Event
    {
        private long _id;
        private DateTime _dateTime;

        public long ID
        {
            get
            {
                return _id;

            }
            set
            {
                _id = value;
            }
        }
        public DateTime DateAndTime
        {
            get
            {
                return _dateTime;

            }
            set
            {
                _dateTime = value;
            }
        }

        /// <summary>
        /// Converts an Event object into a comma separated values string to make it easier to save the stored properties in a CSV file.
        /// </summary>
        /// <returns>CSV string of all properites stored.</returns>
        public virtual string ToCSV()
        {
            return _id + "," + _dateTime;
        }
    }
}
