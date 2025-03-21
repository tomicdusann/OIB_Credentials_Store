﻿using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IAccountManagement
    {
        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        [FaultContract(typeof(InvalidUserException))]
        void CreateAccount(string username, string password);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        [FaultContract(typeof(InvalidUserException))]
        void DeleteAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        [FaultContract(typeof(InvalidUserException))]
        void LockAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        [FaultContract(typeof(InvalidUserException))]
        void EnableAccount(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidGroupException))]
        [FaultContract(typeof(InvalidUserException))]
        void DisableAccount(string username);
    }
}
