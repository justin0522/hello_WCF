using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestDataContract
{
    class MyDataContractSurrogate : IDataContractSurrogate
    {
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public Type GetDataContractType(Type type)
        {
            if (type == typeof(Contact))
            {
                return typeof(Customer);
            }
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            Customer c = obj as Customer;
            if (c == null)
            {
                return obj;
            }
            return new Contact() { FullName = c.FirstName + "," + c.LastName, Sex = c.Gender };
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {

        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            Contact c = obj as Contact;
            if (c == null)
            {
                return obj;
            }
            return new Customer() { FirstName = c.FullName.Split(',')[0], LastName = c.FullName.Split(',')[1], Gender = c.Sex };
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }

    [DataContract(Name = "Customer", Namespace = "http://www.contoso.com")]
    public class Customer
    {
        [DataMember(Order = 1)]
        public string FirstName { get; set; }

        [DataMember(Order = 2)]
        public string LastName { get; set; }

        [DataMember(Order = 3)]
        public string Gender { get; set; }
    }

    public class Contact
    {
        public string FullName { get; set; }

        public string Sex { get; set; }

        public override bool Equals(object obj)
        {
            Contact c = obj as Contact;
            if (c == null)
            {
                return false;
            }
            return this.FullName == c.FullName && this.Sex == c.Sex;
        }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode() ^ this.Sex.GetHashCode();
        }
    }
}
