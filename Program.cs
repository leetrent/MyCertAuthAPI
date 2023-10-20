using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

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
                            httpsOptions.CheckCertificateRevocation = false; // For development.
                            httpsOptions.ClientCertificateValidation = (certificate, chain, errors) =>
                            {
                                // Validate the client's certificate.
                                return certificate.Subject == "CN=client";
                            };
                            // Load the server.pfx for server's SSL/TLS settings
                            httpsOptions.ServerCertificate = new X509Certificate2(@"C:\Users\Lee\Dev\Learning\ChatGPT\ClientCertificateAuth\ExternalResources\server.pfx", "CaseyPo0h");
                        });
                    });
                });
    }
}
