using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestMessage
{
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

    [MessageContract(IsWrapped = true)]
    class BaseData
    {
        [MessageHeader]
        public string Name { get; set; }
        [MessageHeader]
        public int Age { get; set; }
        [MessageBodyMember]
        public string Address { get; set; }
        [MessageBodyMember]
        public string Description { get; set; }
    }

}
