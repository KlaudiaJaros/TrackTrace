using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    /// <summary>
    /// Singleton for saving and retrieving Events data, both Contacts and Visits.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    class EventDataSingleton
    {
        private const string fileName = "EventData.csv"; // a constant file name where the event data is held
        private static long eventId; // to keep track of ids
        private static EventDataSingleton eventDataSystem; // singleton instance

        private EventDataSingleton() { } // private constructor

        /// <summary>
        /// Retrieves the only EventDataSingleton instance:
        /// </summary>
        public static EventDataSingleton EventDataInstance
        {
            get
            {
                if (eventDataSystem == null)
                {
                    eventDataSystem = new EventDataSingleton(); // initialise the singleton if accessed for the first time
                }
                return eventDataSystem;
            }
        }

        /// <summary>
        /// Updates the Id based on the CSV file records to ensure the id corectness after closing the application. 
        /// </summary>
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

        /// <summary>
        /// Saves the given event to a CSV file.
        /// </summary>
        /// <param name="newEvent">An Event to be saved.</param>
        public void SaveEvent(Event newEvent)
        {
            UpdateId(); // refresh id count
            newEvent.ID = eventId; // assign an id
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            // create a string to save:
            string eventCSV = "";
            if (newEvent is Contact)
            {
                // "C" for contacts
                eventCSV = "C," + newEvent.ToCSV();
            }
            else if (newEvent is Visit)
            {
                // "V" for visits:
                eventCSV = "V," + newEvent.ToCSV();
            }

            // save Event:
            File.AppendAllText(path, eventCSV + '\n');
            eventId++;
        }

        /// <summary>
        /// Searches for all users that were recorded in a given location between the given times. 
        /// </summary>
        /// <param name="locationId">ID of a location to search</param>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <returns></returns>
        public List<User> GetUsersByLocationAndDate(long locationId, DateTime fromDate, DateTime toDate)
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            // if the file exists, get all lines and separate each line by a comma:
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');

                    // check the 'type' of an event:
                    char type = separated[0].ElementAt(0);
                    
                    if (type == 'V') // V for Visit:
                    {
                        DateTime date = DateTime.Parse(separated[2]); // get the time and date
                        long recordLocationId = 0;
                        long.TryParse(separated[7], out recordLocationId); // get the id

                        // check if the record is between the desired dates and it is the right id:
                        if (date > fromDate && date < toDate && recordLocationId == locationId)
                        {
                            // create a User:
                            User user = new User();
                            long userId = 0;
                            long.TryParse(separated[3], out userId);
                            user.ID=userId;
                            user.PhoneNumber=separated[4];
                            user.FirstName=separated[5];
                            user.LastName=separated[6];
                            
                            users.Add(user); // save
                        }
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// Searched for all users that were in contact with a given user after a given date.
        /// </summary>
        /// <param name="userId">The user id who people were in contact with</param>
        /// <param name="dateTime">After the date</param>
        /// <returns></returns>
        public List<User> GetUsersByContactAndDate(long userId, DateTime dateTime)
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                // loop trough all the lines and separate them by a comma:
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');
                    
                    // get the 'type':
                    char type = separated[0].ElementAt(0);

                    if (type == 'C') // check if the 'type' is C for Contacts
                    {
                        DateTime date = DateTime.Parse(separated[2]); // get the date and time

                        long user1Id = 0;
                        long.TryParse(separated[3], out user1Id); // get user1 id
                        long user2Id = 0;
                        long.TryParse(separated[7], out user2Id); // get user2 id

                        // check if the record is after the given date and the user match:
                        if (date > dateTime && userId == user1Id) // check the first user
                        {
                            User user2 = new User();
                            user2.ID=user2Id;
                            user2.PhoneNumber=separated[8];
                            user2.FirstName=separated[9];
                            user2.LastName=separated[10];
                            
                            users.Add(user2);
                        }
                        else if (date > dateTime && userId == user2Id) // check the second user
                        {
                            User user1 = new User();
                            user1.ID=user1Id;
                            user1.PhoneNumber=separated[4];
                            user1.FirstName=separated[5];
                            user1.LastName=separated[6];           

                            users.Add(user1);
                        }
                    }
                }
            }
            return users;
        }
        /// <summary>
        /// Get all events that are saved in the data layer.
        /// </summary>
        /// <returns>A list of all events currently stored.</returns>
        public List<Event> GetEvents()
        {
            List<Event> events = new List<Event>(); // to save the event
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Event getEvent = new Event();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] separated = line.Split(','); // separate each line in the file using commas

                    // shared Event properties:
                    char type = separated[0].ElementAt(0);
                    long id = 0;                    
                    long.TryParse(separated[1], out id);
                    getEvent.ID=id;
                    getEvent.DateAndTime=DateTime.Parse(separated[2]);

                    // the first user for both Contact and Visit events:
                    User user1 = new User();
                    long userId = 0;
                    long.TryParse(separated[3], out userId);
                    user1.ID=userId;
                    user1.PhoneNumber=separated[4];
                    user1.FirstName=separated[5];
                    user1.LastName=separated[6];
                    
                    // Contact and Visit specific properties:
                    if (type == 'C') // C for Contacts
                    {
                        // parse the Event into a Contact:
                        Contact contact = (Contact)getEvent;
                        contact.User1 = user1; // save user 1

                        // get the second user for Contact and save it:
                        User user2 = new User();
                        long user2Id = 0;
                        long.TryParse(separated[7], out user2Id);
                        user2.ID=user2Id;
                        user2.PhoneNumber=separated[8];
                        user2.FirstName=separated[9];
                        user2.LastName=separated[10];
                        
                        contact.User2 = user2;
                        getEvent = contact; // save the contact 
                    }
                    else if (type == 'V') // V for Visits
                    {
                        Visit visit = (Visit)getEvent; // parse the Event into a Visit
                        visit.User = user1;

                        // get the location for Visit and save it:
                        Location location = new Location();
                        long locId = 0;
                        long.TryParse(separated[7], out locId);
                        location.ID=locId;
                        location.Name = separated[8];
                        location.Address = separated[9];
                        location.PostCode = separated[10];
                        location.Town = separated[11];

                        visit.Location = location;
                        getEvent = visit; // save the Visit
                    }
                    events.Add(getEvent); // add the Event 
                }
            }
            return events;
        }
    }
}
