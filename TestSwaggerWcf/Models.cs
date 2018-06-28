using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestSwaggerWcf
{
    [DataContract]
    [Description("the pet class ")]
    [SwaggerWcfDefinition("Pet")]
    class Pet
    {
        [DataMember]
        [Description("the unique id ")]
        public int Id { get; set; }

        [DataMember]
        [Description("the pet name ")]
        public string Name { get; set; }

        [DataMember]
        [Description("if the pet is sold ")]
        public bool Sold { get; set; }

        [DataMember]
        [Description("the pet description ")]
        public string Description { get; set; }
    }
}
