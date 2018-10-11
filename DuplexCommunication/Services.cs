using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DuplexCommunication
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class Calculator : ICalculator
    {
        public void Add(int x, int y)
        {
            ICalcullatorCallback callback = OperationContext.Current.GetCallbackChannel<ICalcullatorCallback>();
            int result = x + y;
            callback.OnCallback(result.ToString());

        }
    }
}
