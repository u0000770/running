using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RunnersWcfSL
{

    [DataContract]
    public class RunnerServiceFault
    {
        private string _message;

        public RunnerServiceFault(string message)
        {
            _message = message;
        }

        [DataMember]
        public string Message { get { return _message; } set { _message = value; } }
    }

    //[OperationContract]
    //[FaultContract(typeof(RunnerServiceFault))]
    //void MyServiceOperation();
}
