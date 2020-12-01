using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    public class User
    {
        private long id;
        private string firstName;
        private string lastName;
        private string phoneNo;

        public long GetId()
        {
            return id;
        }
        public string GetPhoneNo()
        {
            return phoneNo;
        }
        public string GetFirstName()
        {
            return firstName;
        }
        public string GetLastName()
        {
            return lastName;
        }
        public void SetFirstName(string fn)
        {
            this.firstName = fn;
        }
        public void SetLastName(string ln)
        {
            this.lastName = ln;
        }
        public void SetId(long i)
        {
            this.id = i;
        }
        public void SetPhoneNo(string p)
        {
            this.phoneNo = p;
        }
        public string ToCSV()
        {
            return this.id + "," + this.phoneNo + "," + this.firstName + "," + this.lastName;
        }
        public override string ToString()
        {
            if (this.firstName.Equals("") && this.lastName.Equals(""))
            {
                return "User: " + this.id;
            }
            else
            {
                string fullname = this.firstName + " " + this.lastName;
                return "User: " + this.id + ", " + fullname;
            }
        }
    }
}
