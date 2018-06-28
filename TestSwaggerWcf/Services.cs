using SwaggerWcf;
using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestSwaggerWcf
{
    [SwaggerWcf("/Pets")]
    [SwaggerWcfServiceInfo(
        title: "SampleService",
        version: "0.0.1",
        Description = "Sample Service to test SwaggerWCF",
        TermsOfService = "Terms of Service"
    )]
    [SwaggerWcfContactInfo(
        Name = "Abel Silva",
        Url = "http://github.com/abelsilva",
        Email = "no@e.mail"
    )]
    [SwaggerWcfLicenseInfo(
        name: "Apache License 2.0",
        Url = "https://github.com/abelsilva/SwaggerWCF/blob/master/LICENSE"
    )]
    class PetsService : SwaggerWcfEndpoint, IPetsService
    {
        private List<Pet> _pets = new List<Pet>() {
            new Pet() { Id= 1001, Name="Cat", Sold= true, Description="black cat" },
            new Pet() { Id=1002, Name="dog", Sold=true, Description="yellow dog" }
        };

        private int _currentId = 1002;

        [SwaggerWcfTag("Pet")]
        //[SwaggerWcfHeader("clientId", false, "Client ID", "000")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as pet title)", true)]
        public int Add(Pet p)
        {
            WebOperationContext woc = WebOperationContext.Current;
            if (woc == null)
                return -1;

            if (p == null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                return -1;
            }

            if (!string.IsNullOrWhiteSpace(p.Name) && p.Name == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return -1;
            }
            p.Id = ++_currentId;
            this._pets.Add(p);
            woc.OutgoingResponse.StatusCode = HttpStatusCode.Created;
            return p.Id;
        }

        //[SwaggerWcfTag("Pet")]
        public bool Delete(string id)
        {
            var temp = this._pets.FirstOrDefault(p => p.Id == Convert.ToInt32(id));
            if (temp == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                this._pets.Remove(temp);
                return true;
            }
        }

        [SwaggerWcfTag("Pet")]
        public List<Pet> GetAll()
        {
            return this._pets;
        }

        [SwaggerWcfTag("Pet")]
        [SwaggerWcfResponse(HttpStatusCode.OK, "Pet found, values in the response body")]
        [SwaggerWcfResponse(HttpStatusCode.NotFound, "Pet not found", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as pet id)", true)]
        public Pet GetById(string id)
        {
            WebOperationContext woc = WebOperationContext.Current;

            if (woc == null)
                return null;

            if (id == "ERROR_500")
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return null;
            }

            var temp = this._pets.FirstOrDefault(p => p.Id == Convert.ToInt32(id));
            if (temp == null)
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
            else
            {
                woc.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return temp;
            }
        }

        //[SwaggerWcfTag("Pet")]
        public bool Update(Pet pet)
        {
            var temp = this._pets.FirstOrDefault(p => p.Id == pet.Id);
            if (temp == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                temp.Name = pet.Name;
                temp.Sold = pet.Sold;
                temp.Description = pet.Description;
                return true;
            }
        }
    }
}
