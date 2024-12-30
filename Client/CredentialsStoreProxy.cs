using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CredentialsStoreProxy : ChannelFactory<IAccountManagement>, IAccountManagement, IDisposable
    {
        IAccountManagement factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void CreateAccount(string username, string password)
        {
            factory.CreateAccount(username, password);
        }

        public void DeleteAccount(string username) 
        { 
            factory.DeleteAccount(username);
        }
    }
}
