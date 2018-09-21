using System;
using Serilog;
using Serilog.Events;
using Serilog.Context;
using core_deamon.enricher;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace core_deamon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
                .Build();
            Log.Information("Starting Service");
            var baseSettings = configbuilder.GetSection("coreSettings").GetChildren();
            //core_deamon.core.logbase.initLog(baseSettings.Key("version"), baseSettings.Key("logfile"))
            Log.Information("Configuration Loaded");

            

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
