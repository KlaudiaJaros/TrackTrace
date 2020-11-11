using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace TrackTrace.BusinessObject
{
    class Visit : Event
    {
        public User User { get; set; }
        public Location Location { get; set; } 
        public override string ToString()
        {
            return "Visit: " + base.GetId().ToString() + ", date: " + base.GetDateTime().ToString() + ", user: " + User.ToString() + ", location: " + Location.GetId() + ", " + Location.Name;
        }
        public override string ToCSV()
        {
            return base.ToCSV() + "," + User.ToCSV() + "," + Location.ToCSV();
        }
    }
}
