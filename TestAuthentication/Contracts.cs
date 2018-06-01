using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestAuthentication
{
    [ServiceContract]
    interface ICalculator
    {
        [OperationContract, WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json
            //UriTemplate = "add/{x}/{y}"
            )]
        int Add(int x, int y);
    }
}
