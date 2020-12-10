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
    /// A UserData Singleton to save and retrieve User objects.
    /// Created by: Klaudia Jaros
    /// Last modified: 09/12/2020
    /// </summary>
    class UserDataSingleton
    {
        private const string _fileName = "UsersData.csv"; // constant file name
        private static long _userId; // to keep track of ids
        private static UserDataSingleton _userDataSystem; // instance of this singleton

        private UserDataSingleton() { } // private constructor

        /// <summary>
        /// Returns the only instance of UserDataSingleton. Static, because it has to be accessible without initialising the object.
        /// </summary>
        public static UserDataSingleton UserDataInstance
        {
            get
            {
                if (_userDataSystem == null)
                {
                    _userDataSystem = new UserDataSingleton(); // initialise the singleton if accessed for the first time
                }
                return _userDataSystem;
            }
        }
        /// <summary>
        /// Updates the id based on the CSV file to ensure the correctness of id after the application closes.
        /// </summary>
        private void UpdateId()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            if (File.Exists(path))
            {
                var lastLine = File.ReadLines(path).Last();
                string[] separated = lastLine.Split(',');
                try
                {
                    _userId = long.Parse(separated[0]) + 1; // get the last id and increment it
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                _userId = 1;
            }
        }

        /// <summary>
        /// Saves a given User objects.
        /// </summary>
        /// <param name="user">User object to be saved.</param>
        public void SaveUser(User user)
        {
            UpdateId();
            user.ID=_userId; // assign a new id
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            File.AppendAllText(path, user.ToCSV() + '\n'); // save
            _userId++;
        }

        /// <summary>
        /// Returns all Users stored in the CSV file.
        /// </summary>
        /// <returns>A list of all users currently stored in the data layer.</returns>
        public Dictionary<long, User> GetUsers()
        {
            Dictionary<long, User> users = new Dictionary<long, User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

            // if the file exists, read all lines, separated each line by a comma:
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    // create a User instance based on the given line:
                    User u = new User();
                    string[] separated = line.Split(',');
                    long.TryParse(separated[0], out long id);
                    u.ID=id;
                    u.PhoneNumber=separated[1];
                    u.FirstName=separated[2];
                    u.LastName=separated[3];
                    
                    users.Add(id,u); // save
                }
            }
            return users;
        }
    }
}
