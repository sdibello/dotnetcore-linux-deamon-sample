using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;
using Serilog.Configuration;
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
                .Enrich.WithProperty("Version", "1.0.0")
                .WriteTo.File("/var/log/core-deamon-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} v{Version:lj} guid{GUID} [{Level:u3}] {Message:lj}{NewLine}{Exception}"                
                    )
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();


            using (LogContext.PushProperty("A", 1))
            {
                LogContext.PushProperty("GUID", Guid.NewGuid());
                Log.Debug("Logging from a property context");
            }


            Log.Information("Starting Service");
            Log.Debug("Hello Debug");

            Log.CloseAndFlush();
            Console.ReadLine();
        }
    }
}
