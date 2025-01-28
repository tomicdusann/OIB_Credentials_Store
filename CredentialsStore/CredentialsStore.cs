using Common;
using Common.Data;
using Common.Exceptions;
using Common.Groups;
using CredentialsStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CredentialsStore
{
    public class CredentialsStore : IAccountManagement
    {
        UsersDB handler = new UsersDB();

        public void CreateAccount(string username, string password)
        {

            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
            {
                User user = new User(username, (password.GetHashCode()).ToString(), false, false, string.Empty, string.Empty);
                List<User> users = handler.getUsers();
                if (users.FindIndex(o => o.GetUsername() == username) == -1)
                {
                    users.Add(user);
                    handler.addUsers(users);
                    Console.WriteLine($"Account - {username} successfully created.");
                }
                else
                    throw new FaultException<InvalidUserException>(new InvalidUserException("This username already exists, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void DeleteAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
            {
                List<User> users = handler.getUsers();
                if ((users.FindIndex(o => o.GetUsername() == username) != -1))
                {
                    users.RemoveAt(users.FindIndex(o => o.GetUsername() == username));
                    handler.addUsers(users);

                    Console.WriteLine($"Account - {username} successfully deleted.");
                }
                else
                    throw new FaultException<InvalidUserException>(new InvalidUserException("This username does not exist, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
        public void EnableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
            {
                List<User> users = handler.getUsers();
                if ((users.FindIndex(o => o.GetUsername() == username) != -1))
                {
                    users[users.FindIndex(o => o.GetUsername() == username)].SetDisabled(false);
                    users[users.FindIndex(o => o.GetUsername() == username)].SetLocked(false);

                    handler.addUsers(users);

                    Console.WriteLine($"Account - {username} succesfully enabled.");
                }
                else
                    throw new FaultException<InvalidUserException>(new InvalidUserException("This username does not exist, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void DisableAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
            {
                List<User> users = handler.getUsers();
                if ((users.FindIndex(o => o.GetUsername() == username) != -1))
                {
                    users[users.FindIndex(o => o.GetUsername() == username)].SetDisabled(true);
                    handler.addUsers(users);

                    Console.WriteLine($"Account - {username} successfully disabled.");
                }
                else
                    throw new FaultException<InvalidUserException>(new InvalidUserException("This username does not exist, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }

        public void LockAccount(string username)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.AdminUser))
            {
                List<User> users = handler.getUsers();
                if ((users.FindIndex(o => o.GetUsername() == username) != -1))
                {
                    users[users.FindIndex(o => o.GetUsername() == username)].SetLocked(true);
                    users[users.FindIndex(o => o.GetUsername() == username)].SetLockedTime();
                    handler.addUsers(users);

                    Console.WriteLine($"Account - {username} succesfully locked.");
                }
                else
                    throw new FaultException<InvalidUserException>(new InvalidUserException("That username does not exists, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
    
    }
}
