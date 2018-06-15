using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TracingLog
{
    class Calculator : ICalculator
    {
        public double Divide(double x, double y)
        {
            if(y == 0)
            {
                //throw new DivideByZeroException("Divide By Zero Exception");
                Calculator.MathError error = new Calculator.MathError("Divide", "Divide By Zero Exception");
                throw new FaultException<Calculator.MathError>(error, new FaultReason("Parameters passed are not valid") { }, new FaultCode("Sender"));
            }
            else
            {
                var result = x / y;
                return result;
            }
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
