using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class UserDataSystem
    {
        private const string fileName = "UsersData.csv"; 
        private static int userId;

        private void UpdateId()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                var lastLine = File.ReadLines(path).Last();
                string[] separated = lastLine.Split(',');
                try
                {
                    userId = Int32.Parse(separated[0]) + 1;
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
            File.AppendAllText(path, user.ToCSV()+'\n');
            userId++;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            User u = new User();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    string[] separated = line.Split(',');
                    int id = 0;
                    Int32.TryParse(separated[0], out id);
                    u.SetId(id);
                    u.SetFirstName(separated[1]);
                    u.SetLastName(separated[2]);
                    u.SetPhoneNo(separated[3]);

                    users.Add(u);
                }
            }
            return users;
        }


    }
}
