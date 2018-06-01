using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    class Program
    {

        static void Main(string[] args)
        {
            //ListAllBindingElements(new WSHttpBinding());
            //GetBindingDefaultValue(new WSHttpBinding());

            //IClientChannel
            string uri = "http://127.0.0.1:9000/calculatorservice";
            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.AddServiceEndpoint(typeof(ICalculator), new SimpleBinding(), "");

            if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = true;
                behavior.HttpGetUrl = new Uri("http://127.0.0.1:9000/calculatorservice/metadata");
                host.Description.Behaviors.Add(behavior);
            }
            try
            {
                host.Open();
                
                ChannelFactory<ICalculatorChannel> factory = new ChannelFactory<ICalculatorChannel>(new SimpleBinding(), uri);
                ICalculatorChannel proxy = factory.CreateChannel();
                Console.WriteLine(proxy.Add(2, 3));

                proxy.Close();// from IClientChannel

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        interface ICalculatorChannel : ICalculator , IClientChannel
        { }

        static void ListAllBindingElements(Binding binding)
        {
            try
            {
                BindingElementCollection elements = binding.CreateBindingElements();
                for (int i = 0; i < elements.Count; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, elements[i].GetType().FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void GetBindingDefaultValue(Binding binding)
        {
            try
            {
                //Type type = typeof(BasicHttpBinding);
                Type type = binding.GetType();
                //var constructor = type.GetConstructor(new Type[] { });
                //var instance = constructor.Invoke(new object[] { });
                var allProperties = type.GetProperties(System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                foreach (var item in allProperties)
                {
                    //Type tt = item.PropertyType;
                    var defaultValue = item.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValue != null)
                    {
                        Console.WriteLine(item.Name + " : " + defaultValue.Value);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
