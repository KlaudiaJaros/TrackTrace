using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    public class Event
    {
        private long id;
        private DateTime dateTime;

        public long GetId()
        {
            return id;
        }
        public void SetId(long i)
        {
            this.id = i;
        }
        public DateTime GetDateTime()
        {
            return dateTime;
        }
        public void SetDateTime(DateTime? d)
        {
            DateTime date = (DateTime)d;
            this.dateTime = date;
        }
        public virtual string ToCSV()
        {
            return this.id + "," + this.dateTime;
        }
    }
}
