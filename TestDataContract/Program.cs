using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestDataContract
{
    class Program
    {
        static void Main(string[] args)
        {

            ExtensibleData();



            //
            MemoryStream ms = new MemoryStream();
            var record = new Record(1, 2, "+", 3);
            var serializer = new DataContractSerializer(typeof(Record));
            serializer.WriteObject(ms, record);
            ms.Position = 0;
            var r = serializer.ReadObject(ms);

            MemoryStream stream2 = new MemoryStream();
            XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream2);
            serializer.WriteObject(binaryDictionaryWriter, record);
            binaryDictionaryWriter.Flush();

            Console.ReadLine();
        }

        static void Serialize<T>(T instance, string fileName, IList<Type> knownTypes, int maxItemsInObjectGraph = int.MaxValue, bool ignoreExtensionDataObject = false, bool preserveObjectReferences = false, IDataContractSurrogate surrogate = null)
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, surrogate);
            using (XmlWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                serializer.WriteObject(writer, instance);
            }
            Process.Start(fileName);
        }

        static T Deserialize<T>(string fileName)
        {
            object obj;
            var serializer = new DataContractSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(fileName))
            {
                obj = serializer.ReadObject(reader);
            }
            return (T)obj;
        }

        static void XMLSerialize<T>(T instancet, string fileName)
        {
            var serialize = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs))
                {
                    serialize.Serialize(writer, instancet);
                }
            }
            Process.Start(fileName);
        }


        static void KnownType()
        {
            ComplexNumber number = new ComplexNumberWithMagnitude(2, 3);
            IList<Type> types = new List<Type>();
            types.Add(typeof(ComplexNumberWithMagnitude));
            Serialize<ComplexNumber>(number, @"justin.xml", types, surrogate: new MyDataContractSurrogate());
        }

        static void ContractSurrogate()
        {
            Contact contact = new Contact();
            contact.FullName = "Justin,Jia";
            contact.Sex = "male";
            Serialize(contact, @"justin.xml", null, surrogate: new MyDataContractSurrogate());
        }

        static void ExtensibleData()
        {
            BaseData data = new BaseData();
            data.Name = "Justin";
            data.Age = 30;
            data.Address = "ChangChun";

            Serialize(data, @"justin.xml", null);
            ExtensibleData myData = Deserialize<ExtensibleData>(@"justin.xml");
            Serialize(data, @"justin2.xml", null);

        }
    }
}
