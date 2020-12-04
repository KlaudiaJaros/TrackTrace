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
    /// </summary>
    class UserDataSingleton
    {
        private const string fileName = "UsersData.csv"; // constant file name
        private static long userId; // to keep track of ids
        private static UserDataSingleton userDataSystem; // instance of this singleton

        private UserDataSingleton() { } // private constructor

        /// <summary>
        /// Returns the only instance of UserDataSingleton.
        /// </summary>
        public static UserDataSingleton UserDataInstance
        {
            get
            {
                if (userDataSystem == null)
                {
                    userDataSystem = new UserDataSingleton(); // initialise the singleton if accessed for the first time
                }
                return userDataSystem;
            }
        }
        /// <summary>
        /// Updates the id based on the CSV file to ensure the correctness of ids after the application closes.
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
                    userId = long.Parse(separated[0]) + 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                userId = 1;
            }
        }

        /// <summary>
        /// Saves a given User objects.
        /// </summary>
        /// <param name="user">User object to be saved.</param>
        public void SaveUser(User user)
        {
            UpdateId();
            user.ID=userId; // assign a new id
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.AppendAllText(path, user.ToCSV() + '\n'); // save
            userId++;
        }

        /// <summary>
        /// Returns all Users stored in the CSV file.
        /// </summary>
        /// <returns>A list of all users currently stored in the data layer.</returns>
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            // if the file exists, read all lines, separated each line by a comma:
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    // create a User instance based on the given line:
                    User u = new User();
                    string[] separated = line.Split(',');
                    long id = 0;
                    long.TryParse(separated[0], out id);
                    u.ID=id;
                    u.PhoneNumber=separated[1];
                    u.FirstName=separated[2];
                    u.LastName=separated[3];
                    
                    users.Add(u); // save
                }
            }
            return users;
        }
    }
}
