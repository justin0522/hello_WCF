using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestDataContract
{
    // Service class which implements the service contract.
    [ServiceBehavior(MaxItemsInObjectGraph = int.MaxValue)]
    [ServiceKnownType(typeof(ComplexNumberWithMagnitude))]
    public class DataContractCalculatorService : IDataContractCalculator
    {
        public ComplexNumber Add(ComplexNumber n1, ComplexNumber n2)
        {
            //Return the derived type
            return new ComplexNumberWithMagnitude(n1.Real + n2.Real, n1.Imaginary + n2.Imaginary);
        }

        public ComplexNumber Subtract(ComplexNumber n1, ComplexNumber n2)
        {
            //Return the derived type
            return new ComplexNumberWithMagnitude(n1.Real - n2.Real, n1.Imaginary - n2.Imaginary);
        }

        public ComplexNumber Multiply(ComplexNumber n1, ComplexNumber n2)
        {
            double real1 = n1.Real * n2.Real;
            double imaginary1 = n1.Real * n2.Imaginary;
            double imaginary2 = n2.Real * n1.Imaginary;
            double real2 = n1.Imaginary * n2.Imaginary * -1;
            //Return the base type
            return new ComplexNumber(real1 + real2, imaginary1 + imaginary2);
        }

        [ServiceKnownType(typeof(ComplexNumberWithMagnitude))]
        public ComplexNumber Divide(ComplexNumber n1, ComplexNumber n2)
        {
            ComplexNumber conjugate = new ComplexNumber(n2.Real, -1 * n2.Imaginary);
            ComplexNumber numerator = Multiply(n1, conjugate);
            ComplexNumber denominator = Multiply(n2, conjugate);
            //Return the base type
            return new ComplexNumber(numerator.Real / denominator.Real, numerator.Imaginary);
        }
    }
}
