using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TestAuthentication
{
    class TransportSecurity
    {
        /// <summary>
        /// Transport -- Basic
        /// </summary>
        public static void UserNameAuthenticationTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetUrl = new Uri("http://localhost:44381/calcullator/metadata"), HttpGetEnabled = true });
            host.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic;
            host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Transport;
            wsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Opened += delegate { Console.WriteLine("host open"); };
                host.Open();

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };
                //factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                //factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);

                //set the Credentials
                factory.Credentials.UserName.UserName = "";
                factory.Credentials.UserName.Password = "";

                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Transport -- Certificate
        /// makecert -r -pe -ss My -sr LocalMachine -a sha1 -sky exchange -n cn=MyCA -sv "MyCAPrivate.pvk" MyCA.cer
        /// makecert -pe -ss My -sr LocalMachine -a sha1 -sky exchange -n cn=SignedClientCertificate -iv "MyCAPrivate.pvk" -ic "MyCA.cer"
        /// </summary>
        public static void ClientCertificateTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";
            string ValidationCertificateThumbprint = "ca5c4e009132da4db17a5b8bbde6382eff4f3f99"; //"36da4d400065739d8c5463fa6fcdd40cf6cc08e0";


            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetUrl = new Uri("http://localhost:44381/calcullator/metadata"), HttpGetEnabled = true });
            
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            //host.Credentials.ClientCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(ValidationCertificateThumbprint);

            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            
            host.AddServiceEndpoint(typeof(ICalculator), binding, "");
            try
            {
                host.Opened += delegate { Console.WriteLine("host open"); };
                host.Open();

                EndpointAddress epAddress = new EndpointAddress(new Uri("https://localhost:44380/calcullator"), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, epAddress);
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };
                
                //set the Credentials
                factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, ValidationCertificateThumbprint);
                
                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Transport -- Windows
        /// </summary>
        public static void WindowsAuthenticationTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetUrl = new Uri("http://localhost:44381/calcullator/metadata"), HttpGetEnabled = true });

            host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Negotiate;
            //host.Credentials.WindowsAuthentication.AllowAnonymousLogons = true;
            host.Credentials.WindowsAuthentication.IncludeWindowsGroups = true;

            //WSHttpBinding wsBinding = new WSHttpBinding();
            //wsBinding.Security.Mode = SecurityMode.Transport;
            //wsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            host.AddServiceEndpoint(typeof(ICalculator), binding, "");
            try
            {
                host.Opened += delegate { Console.WriteLine("host open"); };
                host.Open();
                
                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, epAddress);
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };

                //set the Credentials
                factory.Credentials.Windows.ClientCredential = new NetworkCredential("", "", "");                

                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Transport -- Digest
        /// need AD 
        /// </summary>
        static void DigestAuthenticationTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic | AuthenticationSchemes.Digest;
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetUrl = new Uri("http://localhost:44381/calcullator/metadata"), HttpGetEnabled = true });
            //WSHttpBinding wsBinding = new WSHttpBinding();
            //wsBinding.Security.Mode = SecurityMode.Transport;
            //wsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
            //wsBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Digest;
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Digest;

            host.AddServiceEndpoint(typeof(ICalculator), binding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, epAddress);
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };

                //set the Credentials               
                factory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                factory.Credentials.HttpDigest.ClientCredential = new NetworkCredential("", "", "");

                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Transport -- Ntlm
        /// </summary>
        public static void NtlmAuthenticationTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetUrl = new Uri("http://localhost:44381/calcullator/metadata"), HttpGetEnabled = true });

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.WindowsAuthentication.AllowAnonymousLogons = true;
            host.Credentials.WindowsAuthentication.IncludeWindowsGroups = true;

            //WSHttpBinding wsBinding = new WSHttpBinding();
            //wsBinding.Security.Mode = SecurityMode.Transport;
            //wsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Ntlm;

            host.AddServiceEndpoint(typeof(ICalculator), binding, "");
            try
            {
                host.Opened += delegate { Console.WriteLine("host open"); };
                host.Open();
                
                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(binding, epAddress);
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };

                //set the Credentials
                NetworkCredential credential = factory.Credentials.Windows.ClientCredential;
                credential.Domain = "";
                credential.UserName = "";
                credential.Password = "";

                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// WebHttpBinding -- Transport -- Basic
        /// </summary>
        static void WebHttpBindingTransport()
        {
            string uri = "https://localhost:44380/calcullator";
            string dnsName = "localhost";

            WebServiceHost host = new WebServiceHost(typeof(Calculator), new Uri(uri));

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            //host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            //host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();

            WebHttpBinding webBinding = new WebHttpBinding();
            webBinding.Security.Mode = WebHttpSecurityMode.Transport;
            webBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            host.AddServiceEndpoint(typeof(ICalculator), webBinding, "").Behaviors.Add(new WebHttpBehavior());
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(webBinding, epAddress);
                factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
                //validate the certificate of server
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    Console.WriteLine($"trust the server certificate: {certificate.Subject}");
                    return true;
                };

                //factory.Credentials.UserName.UserName = "";
                //factory.Credentials.UserName.Password = "";                
                factory.Credentials.Windows.ClientCredential = new NetworkCredential("", "", "");

                var proxy = factory.CreateChannel();

                var result = proxy.Add(2, 3);

                Console.WriteLine(result);

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        

    }
}
