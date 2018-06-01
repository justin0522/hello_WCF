using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplexCommunication
{
    class Program
    {
        static void Main(string[] args)
        {
            Duplex();
        }

        static void Duplex()
        {
            string uri = "http://localhost:9000/calcullator";
            var binding = new WSDualHttpBinding();

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.AddServiceEndpoint(typeof(ICalculator), binding, "");
            try
            {
                host.Open();

                InstanceContext context = new InstanceContext(new CalcullatorCallback());
                DuplexChannelFactory<ICalculator> factory = new DuplexChannelFactory<ICalculator>(context, binding, uri);
                ICalculator proxy = factory.CreateChannel();
                proxy.Add(2, 3);

                Console.WriteLine("waiting for callback");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    class CalcullatorCallback : ICalcullatorCallback
    {
        public void OnCallback(string message)
        {
            Console.WriteLine(message);
            Thread.Sleep(1000);
        }
    }
}
