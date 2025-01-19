using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IAuthenticationServiceManagement
    {
        [OperationContract]
        int ValidateCredentials(byte[] username, byte[] password, byte[] signature);
        [OperationContract]
        int ResetUserOnLogOut(byte[] username, byte[] signature);
        [OperationContract]
        int CheckIn(byte[] username, byte[] signature);
    }
}
