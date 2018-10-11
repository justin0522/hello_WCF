using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace TestAuthentication
{
    class MessageSecurity
    {

        /// <summary>
        /// Message -- UserName
        /// </summary>
        public static void UserNameAuthentication()
        {
            string uri = "http://localhost:44380/calcullator";
            string dnsName = "localhost";
            string MessageCertificateThumbprint = "eab03d58da235a737c94b4ed59ef1acb02dd544b";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));

            //encrypt the message
            host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, MessageCertificateThumbprint);

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Message;
            wsBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);
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
        /// Message -- MembershipProvider
        /// </summary>
        public static void MembershipProviderAuthentication()
        {
            string uri = "http://localhost:44380/calcullator";
            string dnsName = "localhost";
            string MessageCertificateThumbprint = "eab03d58da235a737c94b4ed59ef1acb02dd544b";

            var SqlRoleProvider = "SqlRoleProvider"; // find in <system.web>
            var SqlMembershipProvider = "SqlMembershipProvider"; // find in <system.web>

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));

            //encrypt the message
            host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, MessageCertificateThumbprint);

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.MembershipProvider;
            host.Credentials.UserNameAuthentication.MembershipProvider = new System.Web.Security.SqlMembershipProvider() { ApplicationName = SqlMembershipProvider };

            host.Authorization.PrincipalPermissionMode = System.ServiceModel.Description.PrincipalPermissionMode.UseAspNetRoles;
            host.Authorization.RoleProvider = new System.Web.Security.SqlRoleProvider() { ApplicationName = SqlRoleProvider };

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Message;
            wsBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);
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
        /// Message -- Certificate
        /// </summary>
        public static void ClientCertificate()
        {
            string uri = "http://localhost:44380/calcullator";
            string dnsName = "localhost";
            string MessageCertificateThumbprint = "eab03d58da235a737c94b4ed59ef1acb02dd544b";
            string ValidationCertificateThumbprint = "36da4d400065739d8c5463fa6fcdd40cf6cc08e0";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));

            //encrypt the message
            host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, MessageCertificateThumbprint);

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            //host.Credentials.ClientCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(ValidationCertificateThumbprint);

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Message;
            wsBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);
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
        /// Message -- Windows
        /// </summary>
        public static void WindowsAuthentication()
        {
            string uri = "http://localhost:44380/calcullator";
            string dnsName = "localhost";
            string MessageCertificateThumbprint = "eab03d58da235a737c94b4ed59ef1acb02dd544b";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));

            //encrypt the message
            host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, MessageCertificateThumbprint);

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.WindowsAuthentication.AllowAnonymousLogons = true;
            host.Credentials.WindowsAuthentication.IncludeWindowsGroups = true;

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Message;
            wsBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            //without credential negotiation
            //wsBinding.Security.Message.NegotiateServiceCredential = false;
            //wsBinding.Security.Message.EstablishSecurityContext = false;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);
                //set the Credentials
                var credential = factory.Credentials.Windows.ClientCredential;
                //credential.UserName = "";
                //credential.Password = "";

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
        /// need more research
        /// </summary>
        static void IssuedTokenAuthentication()
        {
            string uri = "http://localhost:44380/calcullator";
            string dnsName = "localhost";
            string MessageCertificateThumbprint = "eab03d58da235a737c94b4ed59ef1acb02dd544b";
            string ValidationCertificateThumbprint = "36da4d400065739d8c5463fa6fcdd40cf6cc08e0";

            ServiceHost host = new ServiceHost(typeof(Calculator), new Uri(uri));

            //encrypt the message
            host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, MessageCertificateThumbprint);

            //host.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
            host.Credentials.IssuedTokenAuthentication.RevocationMode = X509RevocationMode.NoCheck;
            host.Credentials.IssuedTokenAuthentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.IssuedTokenAuthentication.CustomCertificateValidator = new CustomX509CertificateValidator(ValidationCertificateThumbprint);

            WSHttpBinding wsBinding = new WSHttpBinding();
            wsBinding.Security.Mode = SecurityMode.Message;
            wsBinding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;

            host.AddServiceEndpoint(typeof(ICalculator), wsBinding, "");
            try
            {
                host.Open();
                Console.WriteLine("host open");

                EndpointAddress epAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity(dnsName));

                ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(wsBinding, epAddress);
                //validate the certificate of server
                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomX509CertificateValidator(MessageCertificateThumbprint);
                //set the Credentials
                factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, ValidationCertificateThumbprint);
                var credential = factory.Credentials.IssuedToken;

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
