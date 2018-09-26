using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace core_deamon.config
{
     public sealed class config
    {
        private static config _instance = null;
        private static readonly object threadLock = new object();
        //just throw some properties in here
        [Required]
        public string version { get; set; }
        [Required]
        public string interations { get; set; }
        [Required]
        public string sleepms { get; set; }
        [Required]
        public string logfile { get; set; }

        config() {
        }

        public static config instance
        {
            get
            {
                //implement thread safe (simple) locking.
                lock (threadLock)
                {
                    if (_instance == null) {
                        _instance = new config();
                    }
                    
                    var result = loadConfigSection();

                    return _instance;
                }
            }
        }

        private static bool loadConfigSection()
        {
            var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var baseSettings = configbuilder.GetSection("coreSettings").GetChildren();
            using (var configseq = baseSettings.GetEnumerator())
            {
                Console.WriteLine(string.Format("reading config {0} - {1}", configseq.Current.Key, configseq.Current.Value));
                while (configseq.MoveNext())
                {
                    Console.WriteLine(string.Format("reading config {0} - {1}", configseq.Current.Key, configseq.Current.Value));
                }
            }
            return true;
        }
    }
}
