using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RunnersWcfSL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRunnerService" in both code and config file together.
    [ServiceContract]
    public interface IRunnerService
    {
       

        [OperationContract]
        List<RunnerDTO> GetAll();

        [OperationContract]
        RunnerDTO GetById(int id);

        [OperationContract]
        bool Add(string firstname,string secondname);

        [OperationContract]
        bool Delete(int id);

        [OperationContract]
        bool Update(RunnerDTO runner);

    }

  


}
