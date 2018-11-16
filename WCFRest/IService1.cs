using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Runner" in both code and config file together.
    [ServiceContract]
    public interface IRunner
    {
        [OperationContract]
        [WebGet(UriTemplate="HelloWorld",RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json)]
        string HelloWorld();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "runners", ResponseFormat = WebMessageFormat.Json)]
        List<Models.RunnerDTO> GetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "runners/{id}", ResponseFormat = WebMessageFormat.Json)]
        RunningModel.runner GetById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "runner", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        bool create(RunningModel.runner runner);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "runner", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]

        bool edit(RunningModel.runner runner);
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "runner", ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        bool delete(string id);




    }







}
