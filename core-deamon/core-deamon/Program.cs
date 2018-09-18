using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;
using Serilog.Configuration;
using core_deamon.enricher;

namespace core_deamon
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With(new threadId_logEventEnricher())
                .Enrich.WithProperty("Version", "1.0.0")
                .WriteTo.File("/var/log/core-deamon-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"                
                    )
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();


            Log.Information("Starting Service");
            Log.Debug("Hello Debug");

            Log.CloseAndFlush();
            Console.ReadLine();
        }
    }
}
