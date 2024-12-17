using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CredentialsStore
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding bindingClient = new NetTcpBinding();
            NetTcpBinding bindingAuthentificationService = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:5000/CredentialsStore";
            string addressAuthentificationService = "net.tcp://localhost:6000/CredentialsStore";

            ServiceHost hostCredentialsStore = new ServiceHost(typeof(CredentialsStore));
            ServiceHost hostAuthenticationServiceManagement = new ServiceHost(typeof(AuthenticationServiceManagement));

            hostCredentialsStore.AddServiceEndpoint(typeof(IAccountManagement), bindingClient, addressClient);
            hostAuthenticationServiceManagement.AddServiceEndpoint(typeof(IAuthenticationServiceManagement), bindingAuthentificationService, addressAuthentificationService);

            hostCredentialsStore.Open();
            hostAuthenticationServiceManagement.Open();

            Console.WriteLine($"Credentials store servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            Console.ReadLine();

            hostCredentialsStore.Close();
            hostAuthenticationServiceManagement.Close();
        }
    }
}
