using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class EventDataSingleton
    {
        private const string fileName = "EventData.csv";
        private static long eventId;
        private static EventDataSingleton eventDataSystem;

        private EventDataSingleton() { }
        public static EventDataSingleton EventDataInstance
        {
            get
            {
                if (eventDataSystem == null)
                {
                    eventDataSystem = new EventDataSingleton();
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
                    eventId = long.Parse(separated[1]) + 1;
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
            if (newEvent is Contact)
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

        public List<User> GetUsersByLocationAndDate(long locationId, DateTime fromDate, DateTime toDate)
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');
                    char type = separated[0].ElementAt(0);
                    if (type == 'V')
                    {
                        DateTime date = DateTime.Parse(separated[2]);
                        long recordLocationId = 0;
                        long.TryParse(separated[7], out recordLocationId);

                        // check if the record is between the desired dates and it is the right location:
                        if (date > fromDate && date < toDate && recordLocationId == locationId)
                        {
                            User user = new User();
                            long userId = 0;
                            long.TryParse(separated[3], out userId);
                            user.SetId(userId);
                            user.SetPhoneNo(separated[4]);
                            user.SetFirstName(separated[5]);
                            user.SetLastName(separated[6]);
                            

                            users.Add(user);
                        }
                    }
                }

            }

            return users;
        }

        public List<User> GetUsersByContactAndDate(long userId, DateTime dateTime)
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');
                    char type = separated[0].ElementAt(0);
                    if (type == 'C')
                    {
                        DateTime date = DateTime.Parse(separated[2]);

                        long user1Id = 0;
                        long.TryParse(separated[3], out user1Id);
                        long user2Id = 0;
                        long.TryParse(separated[7], out user2Id);

                        // check if the record is between the desired dates:
                        if (date > dateTime && userId == user1Id)
                        {
                            User user2 = new User();
                            user2.SetId(user2Id);
                            user2.SetPhoneNo(separated[8]);
                            user2.SetFirstName(separated[9]);
                            user2.SetLastName(separated[10]);
                            

                            users.Add(user2);
                        }
                        else if (date > dateTime && userId == user2Id)
                        {
                            User user1 = new User();
                            user1.SetId(user1Id);
                            user1.SetPhoneNo(separated[4]);
                            user1.SetFirstName(separated[5]);
                            user1.SetLastName(separated[6]);
                            

                            users.Add(user1);
                        }
                    }
                }

            }

            return users;
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
                    long id = 0;                    
                    long.TryParse(separated[1], out id);
                    getEvent.SetId(id);
                    getEvent.SetDateTime(DateTime.Parse(separated[2]));

                    // the first user for both Contact and Visit:
                    User user1 = new User();
                    long userId = 0;
                    long.TryParse(separated[3], out userId);
                    user1.SetId(userId);
                    user1.SetPhoneNo(separated[4]);
                    user1.SetFirstName(separated[5]);
                    user1.SetLastName(separated[6]);
                    

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
                        user2.SetPhoneNo(separated[8]);
                        user2.SetFirstName(separated[9]);
                        user2.SetLastName(separated[10]);
                        

                        contact.User2 = user2;
                        getEvent = contact;
                    }
                    else if (type == 'V')
                    {
                        Visit visit = (Visit)getEvent;
                        visit.User = user1;

                        // get the location for Visit:
                        Location location = new Location();
                        long locId = 0;
                        long.TryParse(separated[7], out locId);
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
