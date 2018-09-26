using System;
using Serilog;
using Serilog.Events;
using Serilog.Context;
using core_deamon.logging;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace core_deamon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var configbuilder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
            //    .Build();

            //core_deamon.core.logbase.initLog(baseSettings.Key("version"), baseSettings.Key("logfile"));
            //Log.Information("Starting Service");
            //var baseSettings = configbuilder.GetSection("coreSettings").GetChildren();
            //using (var configseq = baseSettings.GetEnumerator())
            //{
            //    while (configseq.MoveNext())
            //    {
            //        Log.Information(string.Format("reading config {0} - {1}", configseq.Current.Key, configseq.Current.Value));
            //    }
            //}
            //Log.Information("Configuration Loaded");

            var config = core_deamon.config.config.instance;


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
