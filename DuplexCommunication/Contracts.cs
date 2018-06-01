using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DuplexCommunication
{
    [ServiceContract(SessionMode = SessionMode.Required,
          CallbackContract = typeof(ICalcullatorCallback))]
    interface ICalculator
    {
        [OperationContract(IsOneWay = true)]
        void Add(int x, int y);
    }

    interface ICalcullatorCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCallback(string message);
    }
}
