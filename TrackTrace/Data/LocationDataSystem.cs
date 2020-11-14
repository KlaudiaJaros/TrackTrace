using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class LocationDataSystem
    {
        private const string fileName = "LocationData.csv";
        private static int locationId;

        public void SaveLocation(Location l)
        {
            // TODO: implement
        }

        public List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>();
            // TODO: implement
            return locations;
        }
    }
}
