using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    class SimpleBindingElement : BindingElement
    {
        public SimpleBindingElement()
        {
            //PrintHelper
        }

        public override BindingElement Clone()
        {
            return new SimpleBindingElement();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            return context.GetInnerProperty<T>();
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            return base.BuildChannelFactory<TChannel>(context);
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            return base.BuildChannelListener<TChannel>(context);
        }
    }
}
