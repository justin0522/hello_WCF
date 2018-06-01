using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    class SimpleBinding : Binding
    {
        TransportBindingElement _transportBindingElement = new HttpTransportBindingElement();
        MessageEncodingBindingElement _messageEncodingBindingElement = new TextMessageEncodingBindingElement();
        SimpleBindingElement _simpleBindingElement = new SimpleBindingElement();

        public override string Scheme
        {
            get
            {
                return this._transportBindingElement.Scheme;
            }
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = new BindingElementCollection();
            elements.Add(_simpleBindingElement);
            elements.Add(_messageEncodingBindingElement);
            elements.Add(_transportBindingElement);
            return elements.Clone();
        }
    }
}
