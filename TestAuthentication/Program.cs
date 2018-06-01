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
    class Program
    {
        static void Main(string[] args)
        {
            #region transport
            //TransportSecurity.UserNameAuthenticationTransport();

            //TransportSecurity.ClientCertificateTransport();

            //TransportSecurity.WindowsAuthenticationTransport();

            //TransportSecurity.DigestAuthenticationTransport();

            //TransportSecurity.NtlmAuthenticationTransport();

            //WebHttpBindingTransport();
            #endregion

            #region message
            //MessageSecurity.UserNameAuthentication();

            //MessageSecurity.ClientCertificate();

            MessageSecurity.WindowsAuthentication();
            #endregion

        }







    }
}
