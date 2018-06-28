using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestSwaggerWcf
{
    [ServiceContract]
    interface IPetsService
    {
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/")]
        [SwaggerWcfPath("get all of the pets", "the detail of this operation", "getall")]
        List<Pet> GetAll();

        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/{id}")]
        [SwaggerWcfPath("get pet by id", "the detail of this operation", "getbyid")]
        Pet GetById(string id);

        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "Post",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "add")]
        [SwaggerWcfPath("create new pet", "the detail of this operation", "createone")]
        int Add([SwaggerWcfParameter(Description = "Pet to be created, the id will be replaced")]Pet p);

        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "Delete",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{id}")]
        [SwaggerWcfPath("delete pet from store", "the detail of this operation", "deleteone")]
        bool Delete(string id);

        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "Post",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "update")]
        [SwaggerWcfPath("update the pet", "the detail of this operation", "updateone")]
        bool Update([SwaggerWcfParameter(Description = "Book to be updated, make sure the id of the book exist")]Pet p);

    }
}
