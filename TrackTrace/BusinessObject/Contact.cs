using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    class Contact : Event
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public override string ToString()
        {
            return "Contact: " + base.GetId().ToString() + ", date: " + base.GetDateTime().ToString() +  ", users: " + User1.ToString() + ", " + User2.ToString();
        }
        public override string ToCSV()
        {
            return base.ToCSV() + "," + User1.ToCSV() +"," + User2.ToCSV();
        }
    }
}
