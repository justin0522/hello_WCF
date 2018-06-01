using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace NameNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            var metadata = GetMetadata(typeof(ICalculator));
            var serviceProviderDto = ParseMetadata(metadata);
            GetMessagesInfo(metadata);
            ParseMetadata2(serviceProviderDto[0], typeof(ICalculator));
            SetMetadata(metadata);

            string uri = "http://localhost:9000/calculatorservice";
            Binding binding = new BasicHttpBinding();
            binding.Name = "bindingname";
            binding.Namespace = "http://www.binding.com";

            using (ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri)))
            {
                host.AddServiceEndpoint(typeof(ICalculator), binding, "");

                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri(uri + "/metadata");
                    host.Description.Behaviors.Add(behavior);
                }

                host.Opened += delegate
                {
                    Console.WriteLine("Calculator service is starting ...");
                };

                host.Open();

                Console.ReadLine();
            }

        }

        static MetadataSet GetMetadata(Type serviceType)
        {
            var description = ContractDescription.GetContract(serviceType);
            var exporter = new WsdlExporter();
            exporter.ExportContract(description);
            var metadata = exporter.GetGeneratedMetadata();
            return metadata;
        }

        static void SetMetadata(MetadataSet metadataSet)
        {
            WsdlImporter importer = new WsdlImporter(metadataSet);
            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            // don't known why
            //importer.WsdlImportExtensions.Remove(typeof(XmlSerializerMessageContractImporter));
            //CodeCompileUnit codeCompileUnit = (CodeCompileUnit)importer.State[typeof(CodeCompileUnit)];
            ServiceContractGenerator generator = new ServiceContractGenerator(codeCompileUnit);
            foreach (ContractDescription contract in importer.ImportAllContracts())
            {
                generator.GenerateServiceContractType(contract);
            }

            Assembly assembly = CompilerAssembly(codeCompileUnit);

        }

        static Assembly CompilerAssembly(CodeCompileUnit cUnit)
        {
            CompilerParameters cParameters = new CompilerParameters();
            cParameters.GenerateExecutable = false;
            cParameters.GenerateInMemory = true;
            cParameters.ReferencedAssemblies.Add("System.dll");
            cParameters.ReferencedAssemblies.Add("System.XML.dll");
            cParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
            cParameters.ReferencedAssemblies.Add("System.Data.dll");
            cParameters.ReferencedAssemblies.Add("System.ServiceModel.Web.dll");
            cParameters.OutputAssembly = @"D:\" + DateTime.Now.ToString("hh_mm_ss") + ".dll";
            cParameters.CompilerOptions = "/platform:x86";

            CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
            CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromDom(cParameters, cUnit);
            if (compilerResults.Errors.HasErrors == true)
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError compilerError in compilerResults.Errors)
                {
                    stringBuilder.Append(compilerError.ToString());
                    stringBuilder.Append(System.Environment.NewLine);
                }
                throw new Exception(stringBuilder.ToString());
            }
            //CreateCodeFile(cSharpCodeProvider, codeCompileUnit);
            return compilerResults.CompiledAssembly;
        }

        public static List<ServiceProviderDto> ParseMetadata(MetadataSet metadataSet)
        {
            List<ServiceProviderDto> list = new List<ServiceProviderDto>();
            var wsdlimp = new WsdlImporter(metadataSet);
            var endpoints = wsdlimp.ImportAllEndpoints();

            if (metadataSet.MetadataSections.Count > 0)
            {
                foreach (var section in metadataSet.MetadataSections)
                {
                    var metadata = section.Metadata as System.Web.Services.Description.ServiceDescription;
                    if (metadata != null && metadata.PortTypes.Count > 0)
                    {
                        foreach (System.Web.Services.Description.PortType portType in metadata.PortTypes)
                        {
                            //var endpoint = endpoints.First(a => a.Name == portType.Name);
                            var endpoint = endpoints.FirstOrDefault();

                            if (portType.Operations.Count > 0)
                            {
                                foreach (System.Web.Services.Description.Operation operation in portType.Operations)
                                {
                                    ServiceProviderDto dto = list.SingleOrDefault(o => o.InterfaceName == operation.PortType.Name);

                                    //WsdlOperationMessage operationMessage = new WsdlOperationMessage();
                                    //operationMessage.ActionName = operation.Name;
                                    if (dto != null)
                                    {
                                        dto.Operations.Add(new ServiceOperationDto()
                                        {
                                            OperationName = operation.Name,
                                            //SoapAction = metadata.TargetNamespace + "/" + portType.Name + "/" + operation.Name,
                                            SoapAction = operation.Messages.Input.ExtensibleAttributes.FirstOrDefault(x => x.LocalName == "Action").Value,
                                            SoapReplyAction = operation.Messages.Output.ExtensibleAttributes.FirstOrDefault(x => x.LocalName == "Action").Value,
                                        });
                                    }
                                    else
                                    {
                                        dto = new ServiceProviderDto()
                                        {
                                            InterfaceName = operation.PortType.Name,
                                            ServiceAddress = endpoint?.Address.Uri.ToString()
                                        };
                                        
                                        dto.Operations = new List<ServiceOperationDto>();

                                        dto.Operations.Add(new ServiceOperationDto()
                                        {
                                            OperationName = operation.Name,
                                            SoapAction = operation.Messages.Input.ExtensibleAttributes.FirstOrDefault(x => x.LocalName == "Action").Value,
                                            SoapReplyAction = operation.Messages.Output.ExtensibleAttributes.FirstOrDefault(x => x.LocalName == "Action").Value,
                                        });

                                        list.Add(dto);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return list;
        }

        public static void GetMessagesInfo(MetadataSet metadataSet)
        {
            var wsdlimp = new WsdlImporter(metadataSet);
            var contracts = wsdlimp.ImportAllContracts();
            var operation = contracts[0].Operations[0];

            var message = operation.Messages.FirstOrDefault(x => x.Direction == MessageDirection.Input);
            var result = message.Body.Parts.ToArray();

        }

        public static void ParseMetadata2(ServiceProviderDto dto, Type t)
        {
            MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach(var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(OperationContractAttribute)).FirstOrDefault();
                if(attribute != null)
                {
                    var operationAttri = attribute as OperationContractAttribute;
                    var name = operationAttri.Name;
                    var action = operationAttri.Action;
                    var replyAction = operationAttri.ReplyAction;
                    var operationDto = dto.Operations.FirstOrDefault(o => o.OperationName == (name ?? method.Name) );
                    if(operationDto != null)
                    {
                        operationDto.RealName = method.Name;
                        operationDto.Action = action;
                        operationDto.ReplyAction = replyAction;
                    }
                    else
                    {

                    }
                    Console.WriteLine(operationDto.OperationName);
                }
            }

        }
    }

    [Serializable]
    public class ServiceProviderDto
    {
        public ServiceProviderDto()
        {
            //BindingType = ServiceBindingType.BasicHttpBinding;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        //[DataMember]
        //public ServiceProviderType ProviderType { get; set; }

        //[DataMember]
        //public ServiceProtocolType ProtocolType { get; set; }

        //[DataMember]
        //public ServiceAuthenticationScheme AuthenticationScheme { get; set; }

        //[DataMember]
        //public ServiceCredentialDto Credential { get; set; }

        [DataMember]
        public string PluginName { get; set; }

        [DataMember]
        public Version Version { get; set; }

        [DataMember]
        public string InterfaceNamespace { get; set; }

        [DataMember]
        public string InterfaceName { get; set; }

        /// <summary>
        /// should be interface ns + name
        /// </summary>
        [DataMember]
        public string ServiceFullName { get; set; }

        //[DataMember]
        //public string MetadataXml { get; set; }

        [DataMember]
        public string ServiceAddress { get; set; }

        /// <summary>
        /// External Rest Provider only
        /// </summary>
        [DataMember]
        public List<ServiceOperationDto> Operations { get; set; }

        //[DataMember]
        //public ServiceMetadataSourceDto MetadataDto { get; set; }

        //[DataMember]
        //public ServiceBindingType BindingType { get; set; }

        //[DataMember]
        //public MetadataSourceType MetadataSource { get; set; }

        //[DataMember]
        //public string MetadataLocation { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public long CreatedOn { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public long ModifiedOn { get; set; }



        //public MetadataSet GetMetadata()
        //{
        //    if (this.ProviderType == ServiceProviderType.External && this.ProtocolType == ServiceProtocolType.Rest)
        //    {
        //        return null;
        //    }
        //    return SerializerUtil.DeserializeFromXml<MetadataSet>(this.MetadataXml);
        //}
    }

    [Serializable]
    public class ServiceOperationDto
    {
        public string OperationName { get; set; }
        public string SoapAction { get; set; }
        public string SoapReplyAction { get; set; }

        public string RealName { get; set; }

        public string Action { get; set; }
        public string ReplyAction { get; set; }
    }

}
