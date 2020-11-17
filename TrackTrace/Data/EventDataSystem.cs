using System;
using System.Collections.Generic;
using System.IO;
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
        private static EventDataSystem eventDataSystem;

        private EventDataSystem() { }
        public static EventDataSystem EventDataInstance
        {
            get
            {
                if (eventDataSystem == null)
                {
                    eventDataSystem = new EventDataSystem();
                }
                return eventDataSystem;
            }
        }

        private void UpdateId()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                var lastLine = File.ReadLines(path).Last();
                string[] separated = lastLine.Split(',');
                try
                {
                    eventId = Int32.Parse(separated[1]) + 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                eventId = 1;
            }
        }
        public void SaveEvent(Event newEvent)
        {

            UpdateId();
            newEvent.SetId(eventId);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            string eventCSV = "";
            if(newEvent is Contact)
            {
                eventCSV = "C," + newEvent.ToCSV();
            }
            else if (newEvent is Visit)
            {
                eventCSV = "V," + newEvent.ToCSV();
            }

            File.AppendAllText(path, eventCSV + '\n');
            eventId++;
        }
        public List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Event getEvent = new Event();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');

                    // shared Event properties:
                    char type = separated[0].ElementAt(0);
                    int id = 0;
                    Int32.TryParse(separated[1], out id);
                    getEvent.SetId(id);
                    getEvent.SetDateTime(DateTime.Parse(separated[2]));

                    // the first user for both Contact and Visit:
                    User user1 = new User();
                    int userId = 0;
                    Int32.TryParse(separated[3], out userId);
                    user1.SetId(userId);
                    user1.SetFirstName(separated[4]);
                    user1.SetLastName(separated[5]);
                    user1.SetPhoneNo(separated[6]);

                    // Contact and Visit specific properties:
                    if (type == 'C')
                    {
                        Contact contact = (Contact)getEvent;
                        contact.User1 = user1;

                        // get the second user for Contact:
                        User user2 = new User();
                        int user2Id = 0;
                        Int32.TryParse(separated[7], out user2Id);
                        user2.SetId(user2Id);
                        user2.SetFirstName(separated[8]);
                        user2.SetLastName(separated[9]);
                        user2.SetPhoneNo(separated[10]);

                        contact.User2 = user2;
                        getEvent = contact;
                    }
                    else if (type == 'V')
                    {
                        Visit visit = (Visit)getEvent;
                        visit.User = user1;

                        // get the location for Visit:
                        Location location = new Location();
                        int locId = 0;
                        Int32.TryParse(separated[7], out locId);
                        location.SetId(locId);
                        location.Name = separated[8];
                        location.Address = separated[9];
                        location.PostCode = separated[10];
                        location.Town = separated[11];

                        visit.Location = location;
                        getEvent = visit;
                    }

                    events.Add(getEvent);
                }
            }
            return events;
        }
    }
}
