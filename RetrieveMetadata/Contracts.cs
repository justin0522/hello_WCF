using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveMetadata
{
    [ServiceContract]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        [FaultContract(typeof(MathError))]
        double Divide(double n1, double n2);
    }

    //[DataContract]
    //public class MathError
    //{
    //    private string _operation;
    //    private string _errorMessage;

    //    public MathError(string operation, string errorMessage)
    //    {
    //        this._operation = operation;
    //        this._errorMessage = errorMessage;
    //    }

    //    [DataMember]
    //    public string Operation
    //    {
    //        get { return _operation; }
    //        set { _operation = value; }
    //    }

    //    [DataMember]
    //    public string ErrorMessage
    //    {
    //        get { return _errorMessage; }
    //        set { _errorMessage = value; }
    //    }
    //}
}
