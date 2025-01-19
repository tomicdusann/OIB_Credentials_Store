using AuthenticationService.Data;
using Common;
using Common.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Team9_OIB
{
    class Program
    {
        static void Main(string[] args)
        {

            string secretKey = SecretKey.GenerateKey();
            SecretKey.StoreKey(secretKey, AES.KeyLocation);

            NetTcpBinding bindingClient = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";

            // Windows auth protocol config 

            bindingClient.Security.Mode = SecurityMode.Transport;
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingClient.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;


            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), bindingClient, address);
            host.Open();


            Console.WriteLine($"Authentication service with windows auth successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");


            Console.ReadKey();

            CurrentUsers users = new CurrentUsers();
            users.resetCurrentUsers(); 

            host.Close();
        }
    }
}
