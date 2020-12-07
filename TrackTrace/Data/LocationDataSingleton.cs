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
    /// Singleton for saving and retrieving Location objects.
    /// Created by: Klaudia Jaros
    /// Last modified: 04/12/2020
    /// </summary>
    class LocationDataSingleton
    {
        private const string _fileName = "LocationData.csv"; // constant file name where the locations are being saved
        private static long _locationId; // to keep track of ids
        private static LocationDataSingleton _locationDataSystem; // singleton instance

        private LocationDataSingleton() { } // private constructor

        /// <summary>
        /// Retrieves LocationDataSingleton instance. Static, because it has to be accessible without initialising the object.
        /// </summary>
        public static LocationDataSingleton LocationDataInstance 
        {
            get
            {
                if (_locationDataSystem == null)
                {
                    _locationDataSystem = new LocationDataSingleton(); // initialise the singleton if accessed for the first time
                }
                return _locationDataSystem;
            }
        }

        /// <summary>
        /// Updates the ID based on the CSV file to ensure the correctness of id after the application closes.
        /// </summary>
        private void UpdateId()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            if (File.Exists(path))
            {
                var lastLine = File.ReadLines(path).Last(); // get the last line in the CSV file (last record)
                string[] separated = lastLine.Split(',');
                try
                {
                    _locationId = long.Parse(separated[0]) + 1; // get the last id and increment it
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                _locationId = 1;
            }
        }

        /// <summary>
        /// Saves a given location to the CSV file.
        /// </summary>
        /// <param name="location">Location to be saved.</param>
        public void SaveLocation(Location location)
        {
            UpdateId(); // update the id
            location.ID=_locationId; // assign a new id
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            File.AppendAllText(path, location.ToCSV() + '\n'); // save 
            _locationId++;
        }

        /// <summary>
        /// Returns all location saved in the CSV file.
        /// </summary>
        /// <returns>A list of all locations currently saved in the data layer</returns>
        public List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

            // if the file exists, read all lines, separate each line by a comma:
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    // create a location object based on the given line:
                    Location location = new Location();
                    string[] separated = line.Split(',');
                    long.TryParse(separated[0], out long id);
                    location.ID=id;
                    location.Name = separated[1];
                    location.Address = separated[2];
                    location.PostCode = separated[3];
                    location.Town = separated[4];

                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
