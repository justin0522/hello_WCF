using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    //derived from build-in binding
    class CustomBinding2 : BasicHttpBinding
    {
        public override string Scheme
        {
            get
            {
                return base.Scheme;
            }
        }

        public override BindingElementCollection CreateBindingElements()
        {
            return base.CreateBindingElements();
        }
    }
}
