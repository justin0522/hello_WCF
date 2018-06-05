using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CustomBehavior
{
    [ServiceBehavior]
    class Calculator : ICalculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
