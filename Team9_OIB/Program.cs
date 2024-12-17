using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
            NetTcpBinding bindingClient = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";

            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), bindingClient, address);
            host.Open();


            Console.WriteLine($"Authentication servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");


            Console.ReadKey();
            host.Close();
        }
    }
}
