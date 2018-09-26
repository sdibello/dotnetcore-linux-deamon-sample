using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Context;


namespace core_deamon.logging
{
    public static class logbase
    {
        public static void initLog(string version, string path)
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .Enrich.WithThreadId()
                    .Enrich.WithProperty("Version", version)
                    .WriteTo.File(path,
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} v{Version:lj} [{Level:u3}] [{ThreadId}]({GUID})-{Message:lj}{NewLine}{Exception}"
                        )
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                    .CreateLogger();
        }
    }
}
