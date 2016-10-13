using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;

namespace AltinnDesktopTool.Configuration
{
    public class EnvironmentConfigurationManager
    {
        private static string _configPath = "Configuration\\EnvironmentConfigurations.xml";
        private List<EnvironmentConfiguration> _configurationList;
        public List<EnvironmentConfiguration> EnvironmentConfigurations
        {
            get
            {
                if (_configurationList == null)
                {
                    _configurationList = LoadEnvironmentConfigurations();
                }
                return _configurationList;
            }
        }

        private List<EnvironmentConfiguration> LoadEnvironmentConfigurations()
        {                    
            XElement xmlDoc = XElement.Load(_configPath);
            var configs = from config in xmlDoc.Descendants("EnvironmentConfiguration")
                          select new EnvironmentConfiguration
                          {
                                Name = config.Element("name").Value,
                                ThemeName = config.Element("themeName").Value,
                                ApiKey = config.Element("apiKey").Value,
                                BaseAddress = config.Element("baseAddress").Value,
                                ThumbPrint = config.Element("thumbprint").Value,
                                Timeout = Convert.ToInt32(config.Element("timeout").Value),                                                             
                            };

            return configs.ToList();
        }       
    }
}
