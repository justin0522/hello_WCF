using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace NameNamespace
{
    [ServiceBehavior(Name = "behaviorName", Namespace = "http://www.behavior.com")]
    public class Calculator : ICalculator
    {
        public int Add(int x)
        {
            return ++x;
        }

        public int Add(int x, int y)
        {
            return x + y; 
        }

        public int Add(int x, int y, int z)
        {
            return x + y + z;
        }
    }
}
