using SwaggerWcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestSwaggerWcf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string uri = "http://localhost:5000";

                WebServiceHost host = new WebServiceHost(typeof(PetsService), new Uri(uri));
                WebHttpBinding binding = new WebHttpBinding();

                host.AddServiceEndpoint(typeof(IPetsService), binding, "Pets").Behaviors.Add(new WebHttpBehavior() { HelpEnabled = true });
                //host.AddServiceEndpoint(typeof(ISwaggerWcfEndpoint), binding, "Docs");

                host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });

                host.Opened += delegate { Console.WriteLine("host open"); };
                host.Open();


                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
