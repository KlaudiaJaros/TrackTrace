using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    public static class DataFacade
    {
        private static UserDataSingleton userSystem = UserDataSingleton.UserDataInstance;
        private static LocationDataSingleton locationSystem = LocationDataSingleton.LocationDataInstance;
        private static EventDataSingleton eventSystem= EventDataSingleton.EventDataInstance;

       
        public static void SaveUser(User u)
        {
            userSystem.SaveUser(u);
        }

        public static void SaveLocation(Location l)
        {
            locationSystem.SaveLocation(l);
        }
        public static void SaveEvent(Event e)
        {
            eventSystem.SaveEvent(e);
        }

        public static List<User> GetUsers()
        {
            List<User> users = userSystem.GetUsers();
            return users;
        }
        public static List<Location> GetLocations()
        {
            List<Location> locations = locationSystem.GetLocations();
            return locations;
        }
        public static List<User> GetUsersByLocationAndDate(long locationId, DateTime fromDate, DateTime toDate)
        {
            List<User> users = eventSystem.GetUsersByLocationAndDate(locationId, fromDate, toDate);
            return users;
        }
        public static List<User> GetUsersByContactAndDate(long userId, DateTime dateTime)
        {
            List<User> users = eventSystem.GetUsersByContactAndDate(userId, dateTime);
            return users;
        }
    }
}
