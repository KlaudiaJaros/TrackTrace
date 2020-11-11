using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    class User
    {
        private int id;
        private int phoneNo;

        public List<Event> Events { get; set; } = new List<Event>();

        public int GetId()
        {
            return id;
        }
        public int GetPhoneNo()
        {
            return phoneNo;
        }
        public void SetId(int i)
        {
            this.id = i;
        }
        public void SetPhoneNo(int p)
        {
            this.phoneNo = p;
        }
        public string ToCSV()
        {
            return this.id + "," + this.phoneNo;
        }
    }
}
