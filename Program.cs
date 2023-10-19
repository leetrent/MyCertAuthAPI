using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace MyCertAuthAPI
{
    public class Program
    {
        // Load the CA certificate
        private static readonly X509Certificate2 caCertificate = new X509Certificate2(@"C:\Users\Lee\Dev\Learning\ChatGPT\ClientCertificateAuth\ExternalResources\ca.crt");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                            httpsOptions.ClientCertificateValidation = (certificate, chain, errors) =>
                            {
                                // This example simply checks that the certificate is signed by the trusted CA
                                return certificate.Issuer == caCertificate.Issuer;
                            };
                        });
                    });
                });
    }
}
