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
        private static UserDataSystem userSystem = UserDataSystem.UserDataInstance;
        private static LocationDataSystem locationSystem = LocationDataSystem.LocationDataInstance;
        private static EventDataSystem eventSystem= EventDataSystem.EventDataInstance;

       
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
    }
}
