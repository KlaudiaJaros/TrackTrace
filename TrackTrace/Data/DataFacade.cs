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
    /// Data Layer Facade. It connects all Data Layer Subsytems: User, Location and Event Singletons. It is a static class and can be called 
    /// by any other class within the project to save/retrieve objects.
    /// Created by: Klaudia Jaros
    /// Last modified: 09/12/2020
    /// </summary>
    public static class DataFacade
    {
        // singleton instances:
        private static readonly UserDataSingleton _userSystem = UserDataSingleton.UserDataInstance;
        private static readonly LocationDataSingleton _locationSystem = LocationDataSingleton.LocationDataInstance;
        private static readonly EventDataSingleton _eventSystem= EventDataSingleton.EventDataInstance;

       /// <summary>
       /// Calls UserSystem method to save a User object.
       /// </summary>
       /// <param name="u">User object to be saved.</param>
        public static void SaveUser(User u)
        {
            _userSystem.SaveUser(u);
        }

        /// <summary>
        /// Calls LocationSystem method that saves a Location object.
        /// </summary>
        /// <param name="l">Location object to be saved.</param>
        public static void SaveLocation(Location l)
        {
            _locationSystem.SaveLocation(l);
        }
        /// <summary>
        /// Calls EventSystem method to save an Event.
        /// </summary>
        /// <param name="e">Event object to be saved.</param>
        public static void SaveEvent(Event e)
        {
            _eventSystem.SaveEvent(e);
        }
        /// <summary>
        /// Using UserSystem, retrieved all User objects saved in the DataLayer.
        /// </summary>
        /// <returns>A list of all existing User objects in a form of a Dictionary. Key is user's id.</returns>
        public static Dictionary<long,User> GetUsers()
        {
            Dictionary<long,User> users = _userSystem.GetUsers();
            return users;
        }
        /// <summary>
        /// Using LocationSystem, retrieves all existing Location objects.
        /// </summary>
        /// <returns>A list of all Location objects.</returns>
        public static List<Location> GetLocations()
        {
            List<Location> locations = _locationSystem.GetLocations();
            return locations;
        }
        /// <summary>
        /// Using EventSystem, finds all Users that were recorded in a specific location, between specific date and time.
        /// </summary>
        /// <param name="locationId">Location id for the visit.</param>
        /// <param name="fromDate">From-date</param>
        /// <param name="toDate">To-date</param>
        /// <returns>A list of Users that visited a given location.</returns>
        public static Dictionary<long,User> GetUsersByLocationAndDate(long locationId, DateTime fromDate, DateTime toDate)
        {
            Dictionary<long,User> users = _eventSystem.GetUsersByLocationAndDate(locationId, fromDate, toDate);
            return users;
        }
        /// <summary>
        /// Using EventSystem, finds all Users that were in contact with a given User after a specific date and time.
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="dateTime">After-date</param>
        /// <returns>A list of all users that were in contact with the given User.</returns>
        public static Dictionary<long,User> GetUsersByContactAndDate(long userId, DateTime dateTime)
        {
            Dictionary<long,User> users = _eventSystem.GetUsersByContactAndDate(userId, dateTime);
            return users;
        }

        /// <summary>
        /// Saves the new default user in a CSV file.
        /// </summary>
        /// <param name="u">User to be set as default.</param>
        public static void SetDefaultUser(User u)
        {
            // save the default user in a file so it doesn't dissapear when you reopen the app:
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "defaultUserInfo.csv");
            File.WriteAllText(path, u.ToCSV());
        }

        /// <summary>
        /// Retrieves the default user from a file or returns null if one does not exist.
        /// </summary>
        /// <returns>Default user or null</returns>
        public static User GetDefaultUser()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "defaultUserInfo.csv");

            if (File.Exists(path)) // if the file exists, retrieve the user:
            {
                string[] lines = File.ReadAllLines(path);
                string[] separatedLines = lines[0].Split(',');

                User defaultUser = new User();
                long.TryParse(separatedLines[0], out long id);
                defaultUser.ID = id;
                defaultUser.PhoneNumber = separatedLines[1];
                defaultUser.FirstName = separatedLines[2];
                defaultUser.LastName = separatedLines[3];

                return defaultUser;
            }
            return null;
        }
    }
}
