using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DigitalSignatureHelper
    {
        public static byte[] GenerateDigitalSignature(byte[] data)
        {
            //string signCertificate = DigitalSignatureFormatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";
            X509Certificate2 certificateSign = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "authservice_sign");
            Console.WriteLine($"Certificate used for signing: {certificateSign.ToString()}"); // Using when we use certificates
            return DigitalSignatureManager.Create(data, certificateSign);
        }
        public static bool VerifyDigitalSignature(byte[] data, byte[] signature)
        {
            //string clientName = DigitalSignatureFormatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);// Using when we use certificates
            string clientNameSign = "authservice_sign";
            X509Certificate2 certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientNameSign);
            Console.WriteLine("Got cerfiticate for signing : " + certificate.ToString());
            if (DigitalSignatureManager.Verify(data, signature, certificate))
            {
                Console.WriteLine("Data signature is valid.\n");
                return true;
            }
            else
            {
                Console.WriteLine("Data signature is invalid.\n");
                return false;
            }
        }
    }
}
