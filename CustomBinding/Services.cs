using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    public class Calculator : ICalculator
    {

        public double Add(double x, double y)
        {
            var result = x + y;

            return result;
        }

        public double Divide(double x, double y)
        {
            if (y == 0)
            {
                var detail = new ArgsFault() { FirstArg = x, SecondArg = y };
                var reasion = new FaultReason(new FaultReasonText(""));//Internationalization
                var code = new FaultCode("1");
                throw new FaultException<ArgsFault>(detail, reasion, code, "Divide");
            }
            else
            {
                return x / y;
            }
        }

        public void Multiply(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void Subtract(double x, double y)
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class ArgsFault
    {
        [DataMember]
        public double FirstArg { get; set; }

        [DataMember]
        public double SecondArg { get; set; }
    }
}
