using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
            string credentialsStoreAddress = "net.tcp://localhost:5000/CredentialsStore";

            Console.WriteLine($"Currently used by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");

            using(AuthenticationProxy authentication = new AuthenticationProxy(binding, authenticationServiceAddress))
            {
                using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
                {
                    do {
                        Console.WriteLine("Client started.");
                        Console.ReadLine();
                    }while (true);
                }
            }
        }
    }
}
