using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Data
{
    public class CurrentUsers
    {
        public void addUser(string user)
        {
            StreamWriter sw = new StreamWriter("currentUsers.txt", true, Encoding.ASCII);
            sw.WriteLine(user + '|' + DateTime.Now.ToString("HH:mm:ss"));
            sw.Close();

        }

        public void updateCurrentUsers(List<string> users)
        {
            StreamWriter sw = new StreamWriter("currentUsers.txt", false, Encoding.ASCII);
            foreach (string user in users)
                sw.WriteLine(user);
            sw.Close();
        }

        public void resetCurrentUsers()
        {
            StreamWriter sw = new StreamWriter("currentUsers.txt", false, Encoding.ASCII);
            sw.Close();
        }

        public List<string> getCurrentUsers()
        {
            List<string> currentUsers = new List<string>();
            StreamReader sr = new StreamReader("currentUsers.txt", Encoding.ASCII);

            while (!sr.EndOfStream)
            {
                string user = sr.ReadLine();
                currentUsers.Add(user);
            }

            sr.Close();
            return currentUsers;
        }
    }
}
