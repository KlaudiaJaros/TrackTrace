using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class UserDataSingleton
    {
        private const string fileName = "UsersData.csv";
        private static long userId;
        private static UserDataSingleton userDataSystem;

        private UserDataSingleton() { }

        public static UserDataSingleton UserDataInstance
        {
            get
            {
                if (userDataSystem == null)
                {
                    userDataSystem = new UserDataSingleton();
                }
                return userDataSystem;
            }
        }
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
        public void SaveUser(User user)
        {
            UpdateId();
            user.SetId(userId);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.AppendAllText(path, user.ToCSV() + '\n');
            userId++;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    User u = new User();
                    string[] separated = line.Split(',');
                    long id = 0;
                    long.TryParse(separated[0], out id);
                    u.SetId(id);
                    u.SetPhoneNo(separated[1]);
                    u.SetFirstName(separated[2]);
                    u.SetLastName(separated[3]);
                    

                    users.Add(u);
                }
            }
            return users;
        }
    }
}
