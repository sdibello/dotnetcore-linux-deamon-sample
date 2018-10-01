using System;
using Serilog;
using Serilog.Events;
using Serilog.Context;
using core_deamon.logging;
using core_deamon.config;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace core_deamon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = deamonconfig.instance;
            logbase.initLog("0.0.1", "\\var\\log\\core-deamon\\core-deamon-sample-.log");
            //LogContext.PushProperty("GUID", Guid.NewGuid());
            configLoader.Fill(config);
            Log.Debug("loaded config :{@config}", config);
            Log.Information("Initialization Complete ");


            Console.ReadLine();


            Log.Information("Stopping Service");
            Log.CloseAndFlush();
        }
    }
}
