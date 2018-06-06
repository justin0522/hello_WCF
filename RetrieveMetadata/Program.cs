using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveMetadata
{
    class Program
    {
        const string uri = "http://localhost:8000/service";

        static void Main(string[] args)
        {
            StartService();

            EndpointAddress mexAddress = new EndpointAddress(uri + "/mex");
            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress);

            // Retrieve the metadata for all endpoints using metadata exchange protocol (mex).
            MetadataSet metadataSet = mexClient.GetMetadata();

            //Convert the metadata into endpoints
            WsdlImporter importer = new WsdlImporter(metadataSet);
            ServiceEndpointCollection endpoints = importer.ImportAllEndpoints();

            ContractDescription contract = ContractDescription.GetContract(typeof(ICalculator));
            // Communicate with each endpoint that supports the ICalculator contract.
            foreach (ServiceEndpoint ep in endpoints)
            {
                if (ep.Contract.Namespace.Equals(contract.Namespace) && ep.Contract.Name.Equals(contract.Name))
                {
                    // Create a client using the endpoint address and binding.
                    var client = new ChannelFactory<CalculatorClientChannel>(ep.Binding, new EndpointAddress(ep.Address.Uri)).CreateChannel();

                    // call operations
                    Console.WriteLine(client.Add(2, 3));

                    //Closing the client gracefully closes the connection and cleans up resources
                    client.Close();
                }
            }

            Console.ReadLine();
        }

        static void StartService()
        {
            ServiceHost host = new ServiceHost(typeof(CalculatorService), new Uri(uri));

            host.AddServiceEndpoint(typeof(ICalculator), new WSHttpBinding(), "");

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior() { HttpGetEnabled = true };
            host.Description.Behaviors.Add(behavior);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            host.Opened += delegate { Console.WriteLine("host open"); };

            host.Open();
        }

        static void StartClient()
        {

        }

        interface CalculatorClientChannel : ICalculator, IClientChannel
        {

        }
    }
}
