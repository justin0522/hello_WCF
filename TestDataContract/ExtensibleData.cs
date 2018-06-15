using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestDataContract
{
    [DataContract(Name = "MyData")]
    class BaseData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string Address { get; set; }
    }

    [DataContract(Name = "MyData")]
    class ExtensibleData : IExtensibleDataObject
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }

        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }
    }
}
