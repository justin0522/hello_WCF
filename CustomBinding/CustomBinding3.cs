using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    class CustomBinding3 
    {
        public void Test()
        {
            CustomBinding binding = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpTransportBindingElement());
            binding.Name = "Custom";

        }
    }
}
