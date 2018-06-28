using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveMetadata
{
    [ServiceBehavior]
    public class CalculatorService : ICalculator
    {
        public double Add(double n1, double n2)
        {
            return n1 + n2;
        }

        public double Subtract(double n1, double n2)
        {
            return n1 - n2;
        }

        public double Multiply(double n1, double n2)
        {
            return n1 * n2;
        }

        public double Divide(double n1, double n2)
        {
            if(n2 == 0)
            {
                MathError error = new MathError("Divide", "Divide By Zero Exception");
                throw new FaultException<MathError>(error, new FaultReason("Parameters passed are not valid"));
            }
            var result = n1 / n2;
            return result;
        }

        [DataContract]
        public class MathError
        {
            private string _operation;
            private string _errorMessage;

            public MathError(string operation, string errorMessage)
            {
                this._operation = operation;
                this._errorMessage = errorMessage;
            }

            [DataMember]
            public string Operation
            {
                get { return _operation; }
                set { _operation = value; }
            }

            [DataMember]
            public string ErrorMessage
            {
                get { return _errorMessage; }
                set { _errorMessage = value; }
            }
        }
    }
}
