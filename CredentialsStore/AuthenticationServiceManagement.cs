using Common;
using CredentialsStore.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Config;
using Common.Data;
using Common.Cryptography;

namespace CredentialsStore
{
    public class AuthenticationServiceManagement : IAuthenticationServiceManagement
    {
        UsersDB db = new UsersDB();

        Common.Config.ConfigurationManager config = new Common.Config.ConfigurationManager();

        Dictionary<string, int> failedAttempts = new Dictionary<string, int>();

        // -5 SIGNATURE IS NOT VALID
        // -4 TIMEOUT
        // -3 DISABLED
        // -2 LOCKED
        // -1 IF USER DOES NOT EXISTS
        //  0  IF USER DATA IS VALID
        public int CheckIn(byte[] username, byte[] signature)
        {
            List<User> users = db.getUsers();

            if (DigitalSignatureHelper.VerifyDigitalSignature(username, signature))
            {
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername)
                    {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3; 
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2; 
                        }
                        if (users[i].GetLoggedInTime() == "")
                        {
                            db.addUsers(users);
                            return -4; 
                        }
                        Console.WriteLine($"Account - {outUsername} checked in successfully.\n");
                        db.addUsers(users);
                        return 1;
                    }

                return -1;
            }
            else
                return -5; 
        }

        //RETURNS  0  IF DATA IS RESET
        //RETURNS -1  IF SIGNATURE IS NOT VALID
        public int ResetUserOnLogOut(byte[] username, byte[] signature)
        {
            List<User> users = db.getUsers();

            if (DigitalSignatureHelper.VerifyDigitalSignature(username, signature))
            {
                //Decrypting data
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername)
                    {
                        users[i].SetLoggedTime(string.Empty);
                        db.addUsers(users);
                        break;
                    }
                return 0; 
            }
            else
                return -1;
        }

        public int ValidateCredentials(byte[] username, byte[] password, byte[] signature)
        {
            // -3  DISABLED
            // -2  LOCKED
            // -1  USER DATA DOES NOT EXIST
            // 0   USER DOES NOT EXISTS
            // 1   IF USER DATA IS VALID

            List<User> users = db.getUsers();

            byte[] data = new byte[username.Length + password.Length];
            Buffer.BlockCopy(username, 0, data, 0, username.Length);
            Buffer.BlockCopy(password, 0, data, username.Length, password.Length);

            if(DigitalSignatureHelper.VerifyDigitalSignature(data, signature))
            {
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));
                string outPassword = AES.DecryptData(password, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].GetUsername() == outUsername && (users[i].GetPassword() == outPassword.GetHashCode().ToString())) {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3;
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2;
                        }

                        Console.WriteLine($"Account - {outUsername} with password {outPassword} verified successfully.\n");
                        users[i].SetLoggedTime();
                        db.addUsers(users);
                        return 1;
                    }
                    else if (users[i].GetUsername() == outUsername && (users[i].GetPassword() != outPassword.GetHashCode().ToString()))
                    {
                        if (failedAttempts.ContainsKey(outUsername))
                        {
                            failedAttempts[outUsername]++;
                            if (failedAttempts[outUsername] == config.GetFailedAttempts())
                            {
                                failedAttempts.Remove(outUsername);
                                users[i].SetLocked(true);
                                users[i].SetLockedTime();
                                db.addUsers(users);
                                return -2;
                            }

                        }
                        else
                            failedAttempts.Add(outUsername, 1);

                        return 0;
                    }
                }
                    return -1;
            }
            else
                return -4;
        }
    }
}
