using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    class Event
    {
        private int id;
        private DateTime dateTime;

        public int GetId()
        {
            return id;
        }
        public DateTime GetDateTime()
        {
            return dateTime;
        }
        public void SetId(int i)
        {
            this.id = i;
        }
        public void SetDateTime(DateTime d)
        {
            this.dateTime = d;
        }
        public virtual string ToCSV()
        {
            return this.id + "," + this.dateTime;
        }
    }
}
