using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

namespace core_deamon.config
{
    class configLoader
    {

        public static string Fill(object obj)
        {
            if (obj == null) {
                throw new ArgumentNullException(nameof(obj));
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);

            foreach (var property in properties) {
                var required = property.GetCustomAttribute<RequiredAttribute>() != null;
                var value = loadValue(obj, property.Name, required);

                if (!String.IsNullOrEmpty(value)) {
                    object convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, convertedValue);
                }
            }

            return null;
        }

        public static string loadValue(object obj, string field, bool required = true)
        {
            if (obj == null) {
                throw new ArgumentNullException(nameof(obj));
            }

            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }

            //string settingName = obj.GetType().FullName + "." + field;
            string settingName = field;

            //fix inner classes
            //settingName = settingName.Replace("+", ".");

            var configValue = getConfigValue(settingName, required);
            return configValue;
        }


        /// <summary>
        /// opens the file, tries to find a value based on the key
        /// </summary>
        /// <param name="key">field you are looking for.</param>
        /// <param name="required">will throw a error if it can't find the  value</param>
        /// <returns></returns>
        private static string getConfigValue(string key, bool required = true)
        {
            var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            foreach (var item in configbuilder.AsEnumerable()) {
                if (item.Key == key.Replace('_', ':')) {
                    return item.Value;
                }
            }

            //todo - see if you can get it working without replacing values in the string.. 
            //foreach (var provider in configbuilder.Providers)
            //{
            //    if (provider.GetType().ToString() == "Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider") {
            //        JsonConfigurationProvider jsonProvider = (Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider)provider;

            //    }
            //}

            //todo figure out if we would like it to throw an error
            //todo figure out how to set a better default
            return configbuilder.AsEnumerable().FirstOrDefault(x => x.Key == key).ToString();
        }


    }
}
