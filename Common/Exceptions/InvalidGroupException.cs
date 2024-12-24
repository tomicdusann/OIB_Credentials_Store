using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    [DataContract]
    public class InvalidGroupException
    {
        [DataMember]
        public string exceptionMessage;

        public InvalidGroupException(string exception) { exceptionMessage = exception; }
    }
}
