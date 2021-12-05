using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FireSaverApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //   "applicationUrl": "https://localhost:5001;http://localhost:5000",
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        opt.ListenAnyIP(5001, listenOpt =>
                        {
                            listenOpt.UseHttps(
                            HostConfig.CertificateFileLocation,
                            HostConfig.CertificatePassword);
                        });
                        opt.ListenAnyIP(5000);
                    });

                    webBuilder.UseStartup<Startup>();
                })
                 .ConfigureServices((context, services) =>
                {
                    HostConfig.CertificateFileLocation =
                        context.Configuration["CertificateFileLocation"];
                    HostConfig.CertificatePassword =
                        context.Configuration["CertPassword"];
                })
                .ConfigureServices(services => services.AddHostedService<GuestsCleaner>());
    }


    public static class HostConfig
    {
        public static string CertificateFileLocation { get; set; }
        public static string CertificatePassword { get; set; }
    }
}
