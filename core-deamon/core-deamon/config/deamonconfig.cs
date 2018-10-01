using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace core_deamon.config
{
     public sealed class deamonconfig
    {
        private static deamonconfig _instance = null;
        private static readonly object threadLock = new object();

        //just throw some properties in here
        [Required]
        public string coreSettings_version { get; set; }
        [Required]
        public string coreSettings_interations { get; set; }
        [Required]
        public string coreSettings_sleepms { get; set; }
        [Required]
        public string coreSettings_logfile { get; set; }

        deamonconfig() {
        }

        /// <summary>
        /// thread safe singleton instance creation
        /// </summary>
        public static deamonconfig instance
        {
            get
            {
                //implement thread safe (simple) locking.
                lock (threadLock) {
                    if (_instance == null) {
                        _instance = new deamonconfig();
                    }
                    
                    return _instance;
                }
            }
        }



    }
}
