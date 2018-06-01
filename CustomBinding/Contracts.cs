using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CustomBindings
{
    [ServiceContract]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double x, double y);

        [OperationContract]
        void Subtract(double x, double y);

        [OperationContract]
        void Multiply(double x, double y);

        [OperationContract]
        double Divide(double x, double y);
    }
}
