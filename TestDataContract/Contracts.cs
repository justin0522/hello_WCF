using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestDataContract
{
    // Define a service contract.
    [ServiceContract(Namespace = "http://Microsoft.Samples.Data")]
    [ServiceKnownType(typeof(ComplexNumberWithMagnitude))]
    public interface IDataContractCalculator
    {
        [OperationContract]
        ComplexNumber Add(ComplexNumber n1, ComplexNumber n2);
        [OperationContract]
        ComplexNumber Subtract(ComplexNumber n1, ComplexNumber n2);
        [OperationContract]
        ComplexNumber Multiply(ComplexNumber n1, ComplexNumber n2);
        [OperationContract]
        [ServiceKnownType(typeof(ComplexNumberWithMagnitude))]
        ComplexNumber Divide(ComplexNumber n1, ComplexNumber n2);
    }

    [DataContract(Namespace = "http://Microsoft.Samples.Data")]
    [KnownType(typeof(ComplexNumberWithMagnitude))]
    public class ComplexNumber
    {
        [DataMember]
        public double Real;
        [DataMember]
        public double Imaginary;

        public ComplexNumber(double real, double imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
    }

    //Define the data contract for ComplexNumberWithMagnitude
    [DataContract(Namespace = "http://Microsoft.Samples.KnownTypes", IsReference = true)]
    public class ComplexNumberWithMagnitude : ComplexNumber
    {
        public ComplexNumberWithMagnitude(double real, double imaginary) : base(real, imaginary) { }

        [DataMember]
        public double Magnitude
        {
            get { return Math.Sqrt(Imaginary * Imaginary + Real * Real); }
            set { throw new NotImplementedException(); }
        }
    }

    [DataContract(Namespace = "http://Microsoft.Samples.DataContractSerializer")]
    class Record
    {
        private double n1;
        private double n2;
        private string operation;
        private double result;

        internal Record(double n1, double n2, string operation, double result)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.operation = operation;
            this.result = result;
        }

        [DataMember]
        internal double OperandNumberOne
        {
            get { return n1; }
            set { n1 = value; }
        }

        [DataMember]
        internal double OperandNumberTwo
        {
            get { return n2; }
            set { n2 = value; }
        }

        [DataMember]
        internal string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        [DataMember]
        internal double Result
        {
            get { return result; }
            set { result = value; }
        }

        public override string ToString()
        {
            return string.Format("Record: {0} {1} {2} = {3}", n1, operation, n2, result);
        }

    }
}
