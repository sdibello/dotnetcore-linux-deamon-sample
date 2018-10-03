using System.IO;
using System.Threading.Tasks;
using core_deamon.core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace core_deamon
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var config = deamonconfig.instance;
            //logbase.initLog("0.0.1", "\\var\\log\\core-deamon\\core-deamon-sample-.log");
            ////LogContext.PushProperty("GUID", Guid.NewGuid());
            //configLoader.Fill(config);
            //Log.Debug("loaded config :{@config}", config);
            //Log.Information("Initialization Complete ");

            IHost host = new HostBuilder()
                .ConfigureHostConfiguration( configHost => {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) => {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                    configApp.AddJsonFile($"appsettings.json", true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                    configApp.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, configLogging) => {
                    configLogging.AddSerilog(new LoggerConfiguration()
                            .ReadFrom.Configuration(hostContext.Configuration)
                            .CreateLogger());
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddLogging();
                    services.AddHostedService<core_deamon_process>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
