using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
            string credentialsStoreAddress = "net.tcp://localhost:5000/CredentialsStore";

            //Windows auth tcp protocol init

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;


            Console.WriteLine($"Client with windows auth started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");

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
