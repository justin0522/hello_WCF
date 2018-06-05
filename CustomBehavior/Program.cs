using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CustomBehavior
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:6000/service";
            try
            {
                ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
                BasicHttpBinding binding = new BasicHttpBinding();
                host.AddServiceEndpoint(typeof(ICalculator), binding, "");

                host.Description.Behaviors.Add(new MyServiceBehavior());

                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri(uri + "/mex");
                    host.Description.Behaviors.Add(behavior);
                }
                //host.Opened += delegate { Console.WriteLine("host open"); };
                //host.Opened += delegate (object x, EventArgs y) { Console.WriteLine("host open"); };
                host.Opened += (x, y) => { Console.WriteLine("host open"); };
                host.Open();

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, new EndpointAddress(uri));
                factory.Endpoint.EndpointBehaviors.Add(new MyClientBehavior());

                var proxy = factory.CreateChannel();
                Console.WriteLine(proxy.Add(2, 3));

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
