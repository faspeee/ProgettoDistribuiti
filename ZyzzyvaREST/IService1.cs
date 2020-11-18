using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ZyzzyvaREST
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/getfibonacci",Method = "POST")]
        int GetFibonacci(int number);
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/getmembers", Method = "POST")]
        List<string> GetClusterMembers();

    }
    
}
