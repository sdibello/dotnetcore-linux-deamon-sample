using System.IO;
using System.Threading.Tasks;
using core_deamon.core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                .ConfigureServices((hostContext, services) => {
                    services.AddLogging();
                    services.AddHostedService<core_deamon_process>();
                })
                .ConfigureLogging((hostContext, configLogging) => {
                    configLogging.AddSerilog(new LoggerConfiguration()
                            .ReadFrom.Configuration(hostContext.Configuration)
                            .Enrich.FromLogContext()
                            .Enrich.WithThreadId()
                            .WriteTo.File("{Logging:PathFormat}",
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true,
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}]-{Message:lj}{NewLine}{Exception}")
                            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                            .CreateLogger());
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
