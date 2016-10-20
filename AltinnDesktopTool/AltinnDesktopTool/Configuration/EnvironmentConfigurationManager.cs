using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AltinnDesktopTool.Configuration
{
    /// <summary>
    /// Manages the application environment configuration settings
    /// </summary>
    public class EnvironmentConfigurationManager
    {
        private const string ConfigPath = "Configuration\\EnvironmentConfigurations.xml";
        private static List<EnvironmentConfiguration> configurationList;

        /// <summary>
        /// Returns the configuration settings
        /// </summary>
        public static List<EnvironmentConfiguration> EnvironmentConfigurations => configurationList ?? (configurationList = LoadEnvironmentConfigurations());

        private static List<EnvironmentConfiguration> LoadEnvironmentConfigurations()
        {                    
            var xmlDoc = XElement.Load(ConfigPath);
            var configs = from config in xmlDoc.Descendants("EnvironmentConfiguration")
                          select new EnvironmentConfiguration
                          {
                                Name = config?.Element("name")?.Value,
                                ThemeName = config?.Element("themeName")?.Value,
                                ApiKey = config?.Element("apiKey")?.Value,
                                BaseAddress = config?.Element("baseAddress")?.Value,
                                ThumbPrint = config?.Element("thumbprint")?.Value,
                                Timeout = ParseInt(config?.Element("timeout")?.Value)
                            };
            return configs.ToList();
        }

        private static int ParseInt(string value)
        {
            int ret;
            return value == null ? 0 : int.TryParse(value, out ret) ? ret : 0;
        }
    }
}
