using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;
using Serilog.Configuration;
using Serilog.Enrichers;
using Serilog.Context;
using core_deamon.enricher;

namespace core_deamon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With(new threadId_logEventEnricher())
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Version", "1.0.0")
                .WriteTo.File("/var/log/core-deamon-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} v{Version:lj} [{Level:u3}] [{ThreadId}]({GUID})-{Message:lj}{NewLine}{Exception}"
                    )
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();


            Log.Information("Starting Service");
            using (LogContext.PushProperty("GUID", Guid.NewGuid()))
            {
                Log.Debug("Logging from a property context");
            }

            Log.Debug("Hello Debug");

            Console.ReadLine();
            Log.Information("Stopping Service");
            Log.CloseAndFlush();
        }
    }
}
