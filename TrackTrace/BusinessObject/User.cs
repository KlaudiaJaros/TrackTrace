using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    public class User
    {
        private int id;
        private string firstName;
        private string lastName;
        private string phoneNo;

        public List<Event> Events { get; set; } = new List<Event>();
        public int GetId()
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
        public void SetId(int i)
        {
            this.id = i;
        }
        public void SetPhoneNo(string p)
        {
            this.phoneNo = p;
        }
        public string ToCSV()
        {
            return this.id + "," + this.firstName + "," + this.lastName + "," + this.phoneNo;
        }
        public override string ToString()
        {
            return "User: "+ this.id +", " + this.firstName +", " + lastName;
        }
    }
}
