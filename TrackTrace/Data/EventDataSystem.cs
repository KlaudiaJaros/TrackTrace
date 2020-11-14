using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class EventDataSystem
    {
        private const string fileName = "EventData.csv";
        private static int eventId;

        public void SaveEvent(Event e)
        {
            // TODO: implement
        }
        public List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();
            // TODO: implement
            return events;
        }
    }
}
