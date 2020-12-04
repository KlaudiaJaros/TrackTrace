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
        private const string fileName = "LocationData.csv"; // constant file name where the locations are being saved
        private static long locationId; // to keep track of ids
        private static LocationDataSingleton locationDataSystem; // singleton instance

        private LocationDataSingleton() { } // private constructor

        /// <summary>
        /// Retrieves LocationDataSingleton instance
        /// </summary>
        public static LocationDataSingleton LocationDataInstance 
        {
            get
            {
                if (locationDataSystem == null)
                {
                    locationDataSystem = new LocationDataSingleton(); // initialise the singleton if accessed for the first time
                }
                return locationDataSystem;
            }
        }

        /// <summary>
        /// Updates the ID based on the CSV file to ensure the correctness of ids after the application closes.
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
                    locationId = long.Parse(separated[0]) + 1;
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

        /// <summary>
        /// Saves a given location to the CSV file.
        /// </summary>
        /// <param name="location">Location to be saved.</param>
        public void SaveLocation(Location location)
        {
            UpdateId(); // update the id
            location.ID=locationId; // assign a new id
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.AppendAllText(path, location.ToCSV() + '\n'); // save 
            locationId++;
        }

        /// <summary>
        /// Returns all location saved in the CSV file.
        /// </summary>
        /// <returns>A list of all locations currently saved in the data layer</returns>
        public List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            // if the file exists, read all lines, separate each line by a comma:
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    // create a location object based on the given line:
                    Location location = new Location();
                    string[] separated = line.Split(',');
                    long id = 0;
                    long.TryParse(separated[0], out id);
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
