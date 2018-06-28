#define ConfigFile

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace TracingLog
{
    class TracingLog
    {

        static void Main(string[] args)
        {
            string uri = "http://10.1.83.11:8889/calc";
            try
            {
                //TraceSource wcf = new TraceSource("System.ServiceModel");
                //wcf.TraceInformation("TracingLog");
                //wcf.Flush();
                //wcf.Switch.Level = SourceLevels.Off;

                ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
                BasicHttpBinding binding = new BasicHttpBinding();
                host.AddServiceEndpoint(typeof(ICalculator), binding, "");

                host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                host.Opened += delegate { Console.WriteLine("host opend"); };
                host.Open();

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, uri);
                ICalculator proxy = factory.CreateChannel();

                var result = proxy.Divide(2 , 0);
                //Trace.WriteLine(result, "Debug");
                //Trace.Flush();
                //Debug.WriteLine(result, "Debug");
                //Debug.Flush();
                
                TraceSource ts = new TraceSource("TracingLog.TracingLog");
                ts.TraceInformation("TracingLog");
                ts.Listeners.Add(new TextWriterTraceListener(""));
                ts.Flush();
                DisplayProperties(ts);

                Console.ReadLine();
            }
            catch (FaultException<Calculator.MathError> ex)
            {
                Calculator.MathError error = ex.Detail;
                Console.WriteLine("An Fault is thrown.\n\tFault code:{0}\n\tFault Reason:{1}\n\tOperation:{2}\n\tMessage:{3}", ex.Code, ex.Reason, error.Operation, error.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void DisplayProperties(TraceSource ts)
        {
            Console.WriteLine("TraceSource name = " + ts.Name);
            Console.WriteLine("TraceSource switch level = " + ts.Switch.Level);
            Console.WriteLine("TraceSource switch = " + ts.Switch.DisplayName);
            SwitchAttribute[] switches = SwitchAttribute.GetAll(typeof(TracingLog).Assembly);
            for (int i = 0; i < switches.Length; i++)
            {
                Console.WriteLine("Switch name = " + switches[i].SwitchName);
                Console.WriteLine("Switch type = " + switches[i].SwitchType);
            }
#if(ConfigFile)
            // Get the custom attributes for the TraceSource.
            Console.WriteLine("Number of custom trace source attributes = "
                + ts.Attributes.Count);
            foreach (DictionaryEntry de in ts.Attributes)
                Console.WriteLine("Custom trace source attribute = "
                    + de.Key + "  " + de.Value);
            // Get the custom attributes for the trace source switch.
            foreach (DictionaryEntry de in ts.Switch.Attributes)
                Console.WriteLine("Custom switch attribute = "
                    + de.Key + "  " + de.Value);
#endif
            Console.WriteLine("Number of listeners = " + ts.Listeners.Count);
            foreach (TraceListener traceListener in ts.Listeners)
            {
                Console.Write("TraceListener: " + traceListener.Name + "\t");
                // The following output can be used to update the configuration file.
                Console.WriteLine("AssemblyQualifiedName = " +
                    (traceListener.GetType().AssemblyQualifiedName));
            }
        }
    }
}
