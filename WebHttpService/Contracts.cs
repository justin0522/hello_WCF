using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WebHttpService
{
    [ServiceContract]
    public interface IService
    {
        [WebInvoke()]
        string New(Student s);

        [WebInvoke()]
        bool Delete(string id);

        [WebInvoke()]
        bool Update(Student s);

        [WebGet(UriTemplate = "{id}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Student Get(string id);

        [WebGet(UriTemplate = "/",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Student> List();

    }
}
