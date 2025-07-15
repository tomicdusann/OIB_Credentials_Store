using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

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

            // Windows auth protocol init

            bindingClient.Security.Mode = SecurityMode.Message;
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingClient.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

            ServiceHost hostCredentialsStore = new ServiceHost(typeof(CredentialsStore));
            ServiceHost hostAuthenticationServiceManagement = new ServiceHost(typeof(AuthenticationServiceManagement));


            // Certificate config

            string serverName = CertificateFormatter.ParseName(WindowsIdentity.GetCurrent().Name);
            bindingAuthentificationService.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostAuthenticationServiceManagement.Credentials.ServiceCertificate.Certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "credentialsstore");

            hostCredentialsStore.AddServiceEndpoint(typeof(IAccountManagement), bindingClient, addressClient);
            hostAuthenticationServiceManagement.AddServiceEndpoint(typeof(IAuthenticationServiceManagement), bindingAuthentificationService, addressAuthentificationService);

            hostCredentialsStore.Open();

            try
            {
                hostAuthenticationServiceManagement.Open();
                
            }
            catch (InvalidOperationException) // if the certificate configuration is not successful
            {
                Console.WriteLine("Certificate check failed. Please provide the correct certificates.\n");
                hostCredentialsStore.Abort(); //close the other open host in case of error
            }
            

            Console.WriteLine($"Credentials store service successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            Console.ReadLine();

            hostCredentialsStore.Close();
            hostAuthenticationServiceManagement.Close();
        }
    }
}
