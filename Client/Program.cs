using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using Common.Exceptions;

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

            string menu = "";
            string username = "";
            string password = "";

            using(AuthenticationProxy authentication = new AuthenticationProxy(binding, authenticationServiceAddress))
            {
                using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
                {
                    do {
                        Console.WriteLine("-----SERVER OPTIONS MENU-----\n");
                        Console.WriteLine("1. Sign In");
                        Console.WriteLine("2. Create Account");
                        Console.WriteLine("3. Delete Account");
                        Console.WriteLine("4. Disable Account");
                        Console.WriteLine("5. Enable Account");
                        Console.WriteLine("6. Lock Account");
                        Console.WriteLine("7. Sign Out");
                        Console.Write("\n-> ");
                        menu = Console.ReadLine();

                        switch (menu)
                        {
                            case "2":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    Console.Write("Account password: ");
                                    password = Console.ReadLine();
                                    credentialsStoreProxy.CreateAccount(username, password);
                                    Console.WriteLine("Account successfully created.\n");
                                    username = "";
                                    break;

                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    break;
                                }
                            case "3":
                                try
                                {
                                    Console.Write("Account username: ");
                                    username = Console.ReadLine();
                                    credentialsStoreProxy.DeleteAccount(username);
                                    Console.WriteLine("Account successfully deleted.\n");
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidGroupException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                                catch (FaultException<InvalidUserException> ex)
                                {
                                    Console.WriteLine(ex.Detail.exceptionMessage);
                                    username = "";
                                    break;
                                }
                        }

                    } while (menu != "quit");
                }
            }
        }
    }
}
