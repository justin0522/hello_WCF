using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace CustomBehavior
{
    class ClientInterpector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var token = GetHeaderValue(reply.Headers, Constant.TokenLabel, Constant.DefaultNamespace);
            Console.WriteLine($"AfterReceiveReply - {token}");
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var userNameHeader = MessageHeader.CreateHeader(Constant.UserNameLabel, Constant.DefaultNamespace, Constant.UserNameValue);
            var passwordHeader = MessageHeader.CreateHeader(Constant.PasswordLabel, Constant.DefaultNamespace, Constant.PasswordValue);

            request.Headers.Add(userNameHeader);
            request.Headers.Add(passwordHeader);

            return null;
        }

        string GetHeaderValue(MessageHeaders headers, string name, string ns)
        {
            //var headers = OperationContext.Current.IncomingMessageHeaders;
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
