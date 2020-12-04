using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    /// <summary>
    /// A User class that stores information about a user: id, phone number and firstname and lastname (if provided).
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    public class User
    {
        private long _id;
        private string _firstName;
        private string _lastName;
        private string _phoneNo;

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
        public string PhoneNumber
        {
            get
            {
                return _phoneNo;
            }
            set
            {
                _phoneNo = value;
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        public string FullName
        {
            get
            {
                return _firstName + " " + _lastName;
            }
        }

        /// <summary>
        /// Converts User object into a comma separated values string to make it easier to save the stored properties in a CSV file.
        /// </summary>
        /// <returns>CSV string of all properites stored.</returns>
        public string ToCSV()
        {
            return _id + "," + _phoneNo + "," + _firstName + "," + _lastName;
        }

        public override string ToString()
        {
            // if first name and last name are null, print just their user id and phone number:
            if (_firstName.Equals("") && _lastName.Equals(""))
            {
                return "User: " + _id + ", tel: " + PhoneNumber;
            }
            else
            {
                // else, print their name/fullname too:
                return "User: " + _id + ", tel: " + PhoneNumber + " " + FullName;
            }
        }
    }
}
