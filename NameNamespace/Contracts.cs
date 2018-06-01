using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NameNamespace
{
    [ServiceContract(Name = "CalService", Namespace = "http://www.contoso.com")]
    interface ICalculator
    {
        [OperationContract(Name = "Add2", Action = "http://www.aaa.com/bbb/ccc")]
        int Add(int x, int y);

        [OperationContract(Name = "Add3")]
        int Add(int x, int y, int z);

        [FaultContract(typeof(int))]
        [OperationContract]
        int Add(int x);
    }
}
