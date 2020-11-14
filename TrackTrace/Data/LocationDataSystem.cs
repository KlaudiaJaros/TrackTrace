using System;
using System.Collections.Generic;
using System.IO;
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

        private void UpdateId()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                var lastLine = File.ReadLines(path).Last();
                string[] separated = lastLine.Split(',');
                try
                {
                    locationId = Int32.Parse(separated[0]) + 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                locationId = 1;
            }
        }
        public void SaveLocation(Location location)
        {
            UpdateId();
            location.SetId(locationId);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.AppendAllText(path, location.ToCSV() + '\n');
            locationId++;
        }

        public List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Location location = new Location();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');
                    int id = 0;
                    Int32.TryParse(separated[0], out id);
                    location.SetId(id);
                    location.Name=separated[1];
                    location.Address=separated[2];
                    location.PostCode=separated[3];
                    location.Town = separated[4];

                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
