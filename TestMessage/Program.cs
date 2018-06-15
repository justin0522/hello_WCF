using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;

namespace TestMessage
{
    class Program
    {
        const string Action = "http://www.contoso.com/action";

        static void Main(string[] args)
        {            
            BaseData data = new BaseData();
            data.Name = "Justin";
            data.Age = 22;
            data.Address = "ChangChun";
            data.Description = "Description";
            
            Message message = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, Action, data);
            WriteMessage(message, "justin.xml");
            Console.ReadLine();
        }

        static void WriteMessage(Message message, string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                message.WriteMessage(writer);
            }

            Process.Start(fileName);
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

        static void Simple()
        {
            Record record = new Record(2, 3, "+", 5);
            Message message = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, Action, record);
            WriteMessage(message, "justin.xml");
        }

        static  void XmlReaderBodyWriter()
        {
            Record record = new Record(2, 3, "+", 5);
            XmlReaderBodyWriter writer = new XmlReaderBodyWriter("justin.xml");
            Message message = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, Action, writer);
            WriteMessage(message, "justin2.xml");
        }

    }

    class XmlReaderBodyWriter : BodyWriter
    {
        string _fileName;
        public XmlReaderBodyWriter(string fileName) : base(true)
        {
            this._fileName = fileName;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            using (XmlReader reader = new XmlTextReader(this._fileName))
            {
                while (!reader.EOF)
                {
                    writer.WriteNode(reader, false);
                }
            }
        }
    }
}
