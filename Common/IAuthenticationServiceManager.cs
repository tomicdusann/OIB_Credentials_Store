using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    internal interface IAuthenticationServiceManager
    {
        [OperationContract]
        void LockAccount(string username);

        [OperationContract]
        void EnableAccount(string username);

        [OperationContract]
        void DisableAccount(string username);
    }
}
