using NameNamespaceClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameNamespaceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CalServiceClient client = new CalServiceClient();
            Console.WriteLine(client.Add2(1, 1));


        }
    }
}
