using AuthenticationService;
using AuthenticationService.Data;
using Common;
using Common.Cryptography;
using Common.Exceptions;
using Common.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Team9_OIB
{
    public class AuthenticationService : IAuthenticationService
    {

        CurrentUsers currentUsers = new CurrentUsers();

        public int Login(string username, string password)
        {
            //-4  SIGNATURE IS NOT VALID
            // -3  USER IS DISABLED
            // -2  USER IS LOCKED
            // -1  USER DATA DOES NOT EXIST
            // 0   USER PASSWORD IS NOT VALID
            // 1   USER DATA IS VALID
            // 2   USER IS ALREADY LOGGED IN

            if(Thread.CurrentPrincipal.IsInRole(Groups.GeneralUser))
            {
                CredentialsStoreProxy credentialsStoreProxy = CredentialsStoreProxy.SingletonInstance();
                try
                {
                    List<string> users = currentUsers.getCurrentUsers();
                    for(int i = 0; i<users.Count();i++)
                    {
                        if (users[i].Split('|')[0].Equals(username))
                        {
                            return 2;
                        }
                    }

                    byte[] outUsername = AES.EncryptData(username, SecretKey.LoadKey(AES.KeyLocation));
                    byte[] outPassword = AES.EncryptData(password, SecretKey.LoadKey(AES.KeyLocation));

                    byte[] data = new byte[outUsername.Length + outPassword.Length];
                    Buffer.BlockCopy(outUsername, 0, data, 0, outUsername.Length);
                    Buffer.BlockCopy(outPassword, 0, data, outUsername.Length, outPassword.Length);

                    byte[] signature = DigitalSignatureHelper.GenerateDigitalSignature(data);

                    int ret = credentialsStoreProxy.ValidateCredentials(outUsername, outPassword, signature);

                    switch(ret)
                    {
                        case -4:
                            Console.WriteLine("Signature check failed, your data may be tampered with. Please contact your system administrator.\n");
                            return -4;
                        case -3:
                            Console.WriteLine($"{username} is DISABLED. Please contact your system administrator.\n");
                            return -3;
                        case -2:
                            Console.WriteLine($"{username} is LOCKED. Please contact your system administrator or wait some time and try again.\n");
                            return -2;
                        case -1:
                            Console.WriteLine($"{username} does not exist in our Database. Please contact your system administrator.\n");
                            return -1;
                        case 0:
                            Console.WriteLine($"Password for {username} is wrong.\n");
                            return 0;
                        default:
                            currentUsers.addUser(username);
                            Console.WriteLine($"{username} successfully logged in.\n");
                            return 1;
                    }
                } 
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                    return -1;
                }
            }
            else
            {
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
            }
        }
        public int Logout(string username)
        {
            // 0  LOGOUT IS SUCCESSFUL
            // 1  LOGOUT IS NOT SUCCESSFUL

            if (Thread.CurrentPrincipal.IsInRole(Groups.GeneralUser))
            {
                List<string> users = currentUsers.getCurrentUsers();

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Split('|')[0].Equals(username)) 
                    {
                        users.RemoveAt(i);
                    }
                }

                currentUsers.updateCurrentUsers(users);

                Console.WriteLine($"{username} successfully logged out.\n");

                CredentialsStoreProxy credentialsStoreProxy = CredentialsStoreProxy.SingletonInstance();

                try
                {
                    byte[] outUsername = AES.EncryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                    byte[] signature = DigitalSignatureHelper.GenerateDigitalSignature(outUsername);

                    int ret = credentialsStoreProxy.ResetUserOnLogOut(outUsername, signature);

                    switch(ret)
                    {
                        case -1:
                            Console.WriteLine("Signature check failed, your data may be tampered with. Please contact your system administrator.\n");
                            return -1;
                        case 0:
                            Console.WriteLine($"{username} logged out and user data in DB is successfully reset.\n");
                            break;
                    }
                }

                catch(InvalidOperationException)
                {
                    Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                    return -1;
                }

                return 0; //LOGOUT
            }

             else
             {
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
             }
        }

        public int CheckIn(string username)
        {
            List<string> users = new List<string>();

            CredentialsStoreProxy credentialsStoreProxy = CredentialsStoreProxy.SingletonInstance();
            try
            {
                byte[] outUsername = AES.EncryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                byte[] signature = DigitalSignatureHelper.GenerateDigitalSignature(outUsername);

                int result = credentialsStoreProxy.CheckIn(outUsername, signature);

                switch (result)
                {
                    case -5:
                        users = currentUsers.getCurrentUsers();
                        for (int i = 0; i < users.Count; i++)
                            if (users[i].Split('|')[0] == username)
                            {
                                users.RemoveAt(i);
                                currentUsers.updateCurrentUsers(users);
                            }
                        Console.WriteLine("Signature check failed, your data may be TAMPERED with. Please contact your system administrator.\n");
                        return -5; 
                    case -4:

                        users = currentUsers.getCurrentUsers();
                        for (int i = 0; i < users.Count; i++)
                            if (users[i].Split('|')[0] == username)
                            {
                                users.RemoveAt(i);
                                currentUsers.updateCurrentUsers(users);
                            }
                        Console.WriteLine($"{username} -> checked, TIMED OUT.\n");
                        return -4; 
                    case -3:
                        users = currentUsers.getCurrentUsers();
                        for (int i = 0; i < users.Count; i++)
                            if (users[i].Split('|')[0] == username)
                            {
                                users.RemoveAt(i);
                                currentUsers.updateCurrentUsers(users);
                            }
                        Console.WriteLine($"{username} -> checked, DISABLED.\n");
                        return -3; 
                    case -2:
                        users = currentUsers.getCurrentUsers();
                        for (int i = 0; i < users.Count; i++)
                            if (users[i].Split('|')[0] == username)
                            {
                                users.RemoveAt(i);
                                currentUsers.updateCurrentUsers(users);
                            }
                        Console.WriteLine($"{username} -> checked, LOCKED.\n");
                        return -2; 
                    case -1:
                        users = currentUsers.getCurrentUsers();
                        for (int i = 0; i < users.Count; i++)
                            if (users[i].Split('|')[0] == username)
                            {
                                users.RemoveAt(i);
                                currentUsers.updateCurrentUsers(users);
                            }
                        Console.WriteLine($"{username} -> could not be checked, is MISSING from DB.\n");
                        return -1; 
                    case 0:
                        Console.WriteLine($"{username} -> checked, no changes.\n");
                        break;
                }
                return 0;
            }

            catch (InvalidOperationException)
            {
                Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                return -1;
            }
        }
    }
}
