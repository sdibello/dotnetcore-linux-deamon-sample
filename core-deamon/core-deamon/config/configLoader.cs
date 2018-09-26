using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace core_deamon.config
{
    class configLoader
    {

        public static string GetValue( object obj, string key, bool required = true)
        {
            if ( key == null)
            {
                //todo - this should be "ApplicationNullException" but I can't find anything like that
                throw new ApplicationException(nameof(key));
            }

            //return value
            var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var baseSettings = configbuilder.GetSection("coreSettings").GetChildren();
            using (var configseq = baseSettings.GetEnumerator())
            {
                if (key == configseq.Current.Value) {
                    return configseq.Current.Value;
                }
            }

            return null;
        }

        public static void Fill(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var required = property.GetCustomAttribute<RequiredAttribute>() != null;
                var value = GetValue(obj, property.Name, required);

                if (!String.IsNullOrEmpty(value))
                {
                    object convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, convertedValue);
                }
            }
        }



    }
}
