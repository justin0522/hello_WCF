using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Claims;

namespace TestAuthentication
{
    class Calculator : ICalculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }

    class CustomUserNamePasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == null || password == null)
            {
                throw new ArgumentNullException("userName or password is null");
            }

            if (userName == "justin" && password == "password")
            {
                Console.WriteLine($"Validate the user: {userName}");
            }
            else
            {
                throw new SecurityTokenException("Invalid Username or Password.");
            }
        }
    }

    class CustomX509CertificateValidator : X509CertificateValidator
    {
        string _thumbprint;
        public CustomX509CertificateValidator(string thumbprint)
        {
            this._thumbprint = thumbprint;
        }

        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate is null");
            }

            if (certificate.Thumbprint.ToLower() == this._thumbprint.ToLower())
            {
                Console.WriteLine("Validate X509Certificate " + certificate.Subject);
            }
            else
            {
                throw new SecurityTokenException("Invalid certificate.");
            }
        }
    }
    
}
