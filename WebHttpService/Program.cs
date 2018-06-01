using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;

namespace WebHttpService
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHttpService();
        }

        static void WebHttpService()
        {
            var uri = "http://localhost:9000/students";
            var binding = new WebHttpBinding();

            WebServiceHost host = new WebServiceHost(typeof(Service), new Uri(uri));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), binding, "");
            ep.Behaviors.Add(new WebHttpBehavior() { HelpEnabled = true });

            try
            {
                host.Open();

                ChannelFactory<IService> factory = new ChannelFactory<IService>(binding, uri);
                factory.Endpoint.Behaviors.Add(new WebHttpBehavior());

                IService proxy = factory.CreateChannel();

                var s = proxy.Get("10000");
                Console.WriteLine(s.Name);

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
