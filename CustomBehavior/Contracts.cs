using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CustomBehavior
{
    [ServiceContract]
    interface ICalculator
    {
        [OperationContract]
        int Add(int x, int y);
    }
}
