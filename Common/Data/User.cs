using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class User
    {
        private string username;

        public string GetUsername()
        {
            return username;
        }

        public void SetUsername(string value)
        {
            username = value;
        }

        private string password;

        public string GetPassword()
        {
            return password;
        }

        public void SetPassword(string value)
        {
            password = value;
        }

        private bool locked;

        public bool GetLocked()
        {
            return locked;
        }

        public void SetLocked(bool value)
        {
            locked = value;
            lockedTime = "";
            loggedInTime = "";
        }

        private bool disabled;

        public bool GetDisabled()
        {
            return disabled;
        }

        public void SetDisabled(bool value)
        {
            disabled = value;
            loggedInTime = "";
            lockedTime = "";
        }

        private string lockedTime = string.Empty;

        public string GetLockedTime()
        {
            return lockedTime;
        }

        public void SetLockedTime()
        {
            lockedTime = DateTime.Now.ToString("HH:mm:ss");
        }

        private string loggedInTime;

        public string GetLoggedInTime()
        {
            return loggedInTime;
        }

        public void SetLoggedTime()
        {
            loggedInTime = DateTime.Now.ToString("HH:mm:ss");
        }

        public void SetLoggedTime(string timeReset)
        {
            loggedInTime = timeReset;
        }


        public User(string username, string password, bool locked, bool disabled, string lockedTime, string loggedInTime)
        {
            this.SetUsername(username);
            this.SetPassword(password);
            this.SetLocked(locked);
            this.SetDisabled(disabled);
            this.lockedTime = lockedTime;
            this.loggedInTime = loggedInTime;
        }
    }
}
