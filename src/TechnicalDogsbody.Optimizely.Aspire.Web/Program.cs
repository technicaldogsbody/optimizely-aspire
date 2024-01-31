using CliWrap;
using CliWrap.Buffered;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TechnicalDogsbody.Optimizely.Aspire.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            var runMode = Environment.GetEnvironmentVariable("RUN_MODE");

            if (isDevelopment && runMode == "SCAFFOLD")
            {
                Console.WriteLine("Creating DB");

                //Wait for DB - must be a better way of doing this...
                Thread.Sleep(15000);

                var ps = await Cli.Wrap("dotnet-episerver")
                    .WithWorkingDirectory("")
                    .WithArguments(new[] { "create-cms-database", "TechnicalDogsbody.Optimizely.Aspire.Web.csproj", "-S", "127.0.0.1", "-U", "sa", "-P", "0pti_R0cks", "-dn", "AspireDB" })
                    .ExecuteBufferedAsync();

                Console.WriteLine(ps.StandardOutput);
                Console.WriteLine(ps.StandardError);

                ps = await Cli.Wrap("dotnet-episerver")
                    .WithWorkingDirectory("")
                    .WithArguments(new[] { "add-admin-user", "TechnicalDogsbody.Optimizely.Aspire.Web.csproj", "-u", "admin", "-p", "P4ssw0rd!", "-e", "local.user@build.local", "-c", "EPiServerDB" })
                    .ExecuteBufferedAsync();

                Console.WriteLine(ps.StandardOutput);
                Console.WriteLine(ps.StandardError);

                File.Create("db.created");

                return;
            }
            else if (runMode == "WEB")
            {
                Console.WriteLine("Ready to roll!");

                //Wait for DB - must be a better way of doing this...
                Thread.Sleep(10000);

                CreateHostBuilder(args, isDevelopment).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, bool isDevelopment)
        {
            if (isDevelopment)
            {
                //Development configuration can be addded here, like local logging.
                return Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(l =>
                    {
                        l.AddOpenTelemetry(logging =>
                        {
                            logging.IncludeFormattedMessage = true;
                            logging.IncludeScopes = true;
                        });
                    })
                    .ConfigureCmsDefaults()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
            }
            else
            {
                return Host.CreateDefaultBuilder(args)
                    .ConfigureCmsDefaults()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
            }
        }
    }
}
