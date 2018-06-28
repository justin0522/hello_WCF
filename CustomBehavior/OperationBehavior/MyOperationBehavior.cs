using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace CustomBehavior
{
    class MyOperationBehaviorAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            Console.WriteLine("Action: " + clientOperation.Action);
            Console.WriteLine("ReplyAction: " + clientOperation.ReplyAction);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //dispatchOperation.CallContextInitializers.Add()
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }
    }
}
