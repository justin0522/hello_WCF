using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CallRestFromWcf
{
    class Program
    {
        static void Main(string[] args)
        {
            CallRestFromWcf();
        }

        static void CallRestFromWcf()
        {
            ServiceHost restHost = new ServiceHost(typeof(RestService), new Uri(Constant.RestServiceBaseAddress));
            restHost.AddServiceEndpoint(typeof(IRestInterface), new WebHttpBinding(), "").Behaviors.Add(new WebHttpBehavior());
            restHost.Open();

            ServiceHost normalHost = new ServiceHost(typeof(NormalService), new Uri(Constant.NormalServiceBaseAddress));
            normalHost.AddServiceEndpoint(typeof(INormalInterface), new BasicHttpBinding(), "");
            normalHost.Open();

            Console.WriteLine("Hosts opened");

            ChannelFactory<INormalInterface> factory = new ChannelFactory<INormalInterface>(new BasicHttpBinding(), new EndpointAddress(Constant.NormalServiceBaseAddress));
            INormalInterface client = factory.CreateChannel();

            Console.WriteLine(client.CallAdd(2, 3));
            Console.WriteLine(client.CallEcho("Hello, World"));

            Console.ReadLine();
        }
    }
}
