using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;

namespace CustomBehavior
{
    class MyServiceBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(MyServiceBehavior);
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher dis in dispatcher.Endpoints)
                {
                    dis.DispatchRuntime.MessageInspectors.Add(new ServiceInterpector());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        protected override object CreateBehavior()
        {
            return new MyServiceBehavior();
        }
    }
}
