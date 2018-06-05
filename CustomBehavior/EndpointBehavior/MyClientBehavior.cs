using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;

namespace CustomBehavior
{
    class MyClientBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(MyClientBehavior);
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ClientInterpector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        protected override object CreateBehavior()
        {
            return new MyClientBehavior();
        }
    }
}
