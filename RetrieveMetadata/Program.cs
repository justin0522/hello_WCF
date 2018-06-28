using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveMetadata
{
    class Program
    {
        const string uri = "http://localhost:8000/service";

        static void Main(string[] args)
        {
            StartService();

            //ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(new WSHttpBinding(), uri);
            //ICalculator proxy = factory.CreateChannel();
            //try
            //{
            //    proxy.Divide(2, 0);
            //}
            //catch (FaultException<MathError> ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //ImportAllContracts();

            //ImportAllEndpoints();

            Console.ReadLine();
        }

        static void StartService()
        {
            ServiceHost host = new ServiceHost(typeof(CalculatorService), new Uri(uri));

            host.AddServiceEndpoint(typeof(ICalculator), new WSHttpBinding(), "");

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior() { HttpGetEnabled = true };
            host.Description.Behaviors.Add(behavior);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            host.Opened += delegate { Console.WriteLine("host open"); };

            host.Open();
        }

        static void ImportAllContracts()
        {
            EndpointAddress mexAddress = new EndpointAddress(uri + "/mex");
            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress);

            // Retrieve the metadata for all endpoints using metadata exchange protocol (mex).
            MetadataSet metadataSet = mexClient.GetMetadata();

            //Convert the metadata into endpoints
            WsdlImporter importer = new WsdlImporter(metadataSet);
            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
           
            var contracts = importer.ImportAllContracts();
            ServiceContractGenerator generator = new ServiceContractGenerator(codeCompileUnit);
            foreach (var item in contracts)
            {
                generator.GenerateServiceContractType(item);
            }
            if (generator.Errors.Count != 0)
            {
                throw new Exception("error");
            }

            CompilerCSharpCode(generator);
            var assembly = CompilerAssembly(codeCompileUnit);

            CompilerNewServiceContract(assembly, importer);



        }

        static void CompilerNewServiceContract(Assembly assembly, WsdlImporter importer)
        {
            //builder
            AssemblyBuilder AssBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("AssemblyAlias"), AssemblyBuilderAccess.Run);
            ModuleBuilder MdlBuilder = AssBuilder.DefineDynamicModule("ModuleName");
            TypeBuilder itfBuilder = MdlBuilder.DefineType("InterfaceName", TypeAttributes.Abstract | TypeAttributes.Interface);
            itfBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(ServiceContractAttribute).GetConstructor(Type.EmptyTypes), new object[0]));

            Type orignalType = assembly.GetType("ICalculator", true, true);
            MethodInfo method = orignalType.GetMethod("Divide");
            ParameterInfo[] paraList = method.GetParameters();
            //method builder
            MethodBuilder mtdBuilder = itfBuilder.DefineMethod("Divide", MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                    method.ReturnType, paraList.Any() ? paraList.Select(p => p.ParameterType).ToArray() : null);
            Collection<ContractDescription> des_contracts = importer.ImportAllContracts();
            OperationDescription des_opt = des_contracts.First(a => a.Name == "ICalculator")
                .Operations.First(b => b.Name == "Divide");

            MessageDescription des_outMsg = des_opt.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Output);
            MessageDescription des_intMsg = des_opt.Messages.FirstOrDefault(m => m.Direction == MessageDirection.Input);

            if (des_intMsg != null && des_intMsg.Body.Parts.Any())
            {
                var parts = des_intMsg.Body.Parts.ToArray();
                /// set parameter alias : Method(T) -> Method(T t)
                for (var i = 0; i < parts.Length; i++)
                {
                    mtdBuilder.DefineParameter(i + 1, ParameterAttributes.In, parts[i].Name);
                }
            }
            //set custom attribute

            var args = typeof(MathError);

            mtdBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(WebInvokeAttribute).GetConstructors()[0], new object[0]));
            var attr = method.GetCustomAttribute<FaultContractAttribute>() as FaultContractAttribute;
            mtdBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(FaultContractAttribute).GetConstructors()[0], new object[] { args }));
            
            var InterfaceType = itfBuilder.CreateType();
            //
            //var methodInfo = InterfaceType.GetMethod("Divide");
            //var t = methodInfo.ReturnType;
            //var att = methodInfo.CustomAttributes;
            //
            ContractDescription description = ContractDescription.GetContract(InterfaceType);
        }

        static void CompilerCSharpCode(ServiceContractGenerator generator)
        {
            string outputFile = @"c:\justin\contracts.cs";
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            IndentedTextWriter textWriter = new IndentedTextWriter(new System.IO.StreamWriter(outputFile));
            provider.GenerateCodeFromCompileUnit(generator.TargetCompileUnit, textWriter, options);
            textWriter.Close();
            //CalculatorClient class
        }

        static Assembly CompilerAssembly(CodeCompileUnit cUnit)
        {
            //设定编译参数
            CompilerParameters cParameters = new CompilerParameters();
            cParameters.GenerateExecutable = false;
            cParameters.GenerateInMemory = true;
            cParameters.ReferencedAssemblies.Add("System.dll");
            cParameters.ReferencedAssemblies.Add("System.XML.dll");
            cParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
            cParameters.ReferencedAssemblies.Add("System.Data.dll");
            cParameters.ReferencedAssemblies.Add("System.ServiceModel.Web.dll");
            cParameters.OutputAssembly = @"C:\Justin\" + DateTime.Now.ToString("hh_mm_ss") + ".dll";
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


        static void ImportAllEndpoints()
        {
            EndpointAddress mexAddress = new EndpointAddress(uri + "/mex");
            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress);

            // Retrieve the metadata for all endpoints using metadata exchange protocol (mex).
            MetadataSet metadataSet = mexClient.GetMetadata();

            //Convert the metadata into endpoints
            WsdlImporter importer = new WsdlImporter(metadataSet);

            #region endpoints
            ServiceEndpointCollection endpoints = importer.ImportAllEndpoints();

            ContractDescription contract = ContractDescription.GetContract(typeof(ICalculator));
            // Communicate with each endpoint that supports the ICalculator contract.
            foreach (ServiceEndpoint ep in endpoints)
            {
                if (ep.Contract.Namespace.Equals(contract.Namespace) && ep.Contract.Name.Equals(contract.Name))
                {
                    // Create a client using the endpoint address and binding.
                    var client = new ChannelFactory<CalculatorClientChannel>(ep.Binding, new EndpointAddress(ep.Address.Uri)).CreateChannel();

                    // call operations
                    Console.WriteLine(client.Add(2, 3));

                    //Closing the client gracefully closes the connection and cleans up resources
                    client.Close();
                }
            }
            #endregion
        }

        static void StartClient()
        {

        }

        interface CalculatorClientChannel : ICalculator, IClientChannel
        {

        }
    }
}
