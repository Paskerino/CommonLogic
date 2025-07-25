using CommonLogic.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace CommonLogic.Services.Workers
{


    public class ConfigurationLoader
    {
        public List<Device> LoadDevices()
        {
            string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(configFilePath))
            {
                // Просто кидаємо виняток. UI його зловить і обробить.
                throw new FileNotFoundException("Файл конфігурації appsettings.json не знайдено.", configFilePath);
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration.GetSection("Devices").Get<List<Device>>() ?? new List<Device>();
        }
        public List<TriggerRule> LoadTriggerRules()
        {
            string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(configFilePath))
            {
                // Просто кидаємо виняток. UI його зловить і обробить.
                throw new FileNotFoundException("Файл конфігурації appsettings.json не знайдено.", configFilePath);
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration.GetSection("TriggerRules").Get<List<TriggerRule>>() ?? new List<TriggerRule>();
        }
    }
}
