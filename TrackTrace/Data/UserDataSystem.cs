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
        private static int userId=0;

        public void SaveUser(User u)
        {
            // TODO: existing files
            u.SetId(userId);
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.WriteAllText(destPath, u.ToCSV());
            userId++;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            // TODO: implement
            return users;
        }


    }
}
