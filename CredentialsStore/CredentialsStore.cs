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
                    throw new FaultException<InvalidUserException>(new InvalidUserException("That username already exists, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
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
                    throw new FaultException<InvalidUserException>(new InvalidUserException("That username does not exists, please try again.\n"));
            }
            else
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions, please contact your system administrator if you think this is a mistake.\n"));
        }
    }
}
