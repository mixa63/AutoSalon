using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    internal static class ConfigManager
    {
        private static readonly IConfigurationRoot _configuration;
        public static AppConfig AppConfig { get; }

        static ConfigManager()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            AppConfig = new AppConfig
            {
                Ip = _configuration["AppConfig:Ip"],
                Port = int.Parse(_configuration["AppConfig:Port"] ?? "7000"),
                ConnectionString = _configuration["AppConfig:ConnectionString"]
            };
        }
    }
}
