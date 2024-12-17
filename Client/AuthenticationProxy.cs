using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AuthenticationProxy : ChannelFactory<IAuthenticationService>, IAuthenticationService, IDisposable
    {
        IAuthenticationService factory;

        public AuthenticationProxy(NetTcpBinding binding, string address) : base(binding, address) 
        {
            factory = this.CreateChannel();
        }

        public void InitialFunction()
        {
            throw new NotImplementedException();
        }

        //TO DO:
    }
}
