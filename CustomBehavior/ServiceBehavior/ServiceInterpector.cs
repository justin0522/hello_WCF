using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace CustomBehavior
{
    class ServiceInterpector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Console.WriteLine(request);
            var user = GetHeaderValue(Constant.UserNameLabel, Constant.DefaultNamespace);
            var pwd = GetHeaderValue(Constant.PasswordLabel, Constant.DefaultNamespace);
            if (user != Constant.UserNameValue || pwd != Constant.PasswordValue)
            {
                throw new FaultException("Invalid User");
            }
            
            return Guid.NewGuid();
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if(correlationState is Guid)
            {
                var id = Guid.Parse(correlationState.ToString());
                var tokenHeader = MessageHeader.CreateHeader(Constant.TokenLabel, Constant.DefaultNamespace, id);
                reply.Headers.Add(tokenHeader);
                Console.WriteLine($"BeforeSendReply - {id}");
            }
        }

        string GetHeaderValue(string name, string ns)
        {
            var headers = OperationContext.Current.IncomingMessageHeaders;
            var index = headers.FindHeader(name, ns);
            if (index >= 0)
            {
                return headers.GetHeader<string>(index);
            }
            else
            {
                return null;
            }
        }
    }
}
