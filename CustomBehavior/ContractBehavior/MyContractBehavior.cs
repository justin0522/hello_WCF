using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace CustomBehavior
{
    class MyContractBehaviorAttribute : Attribute, IContractBehaviorAttribute, IContractBehavior
    {
        public Type TargetContract
        {
            get
            {
                return typeof(ICalculator);
            }
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach(EndpointDispatcher dis in dispatchRuntime.ChannelDispatcher.Endpoints)
            {
                dis.DispatchRuntime.MessageInspectors.Add(new ServiceInterpector());
            }
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {

        }
    }
}
