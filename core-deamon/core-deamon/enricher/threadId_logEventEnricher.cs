using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace core_deamon.enricher
{
    class threadId_logEventEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
