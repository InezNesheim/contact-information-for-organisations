using System;

using AltinnDesktopTool.Configuration;

using RestClient;
using System.Linq;

namespace AltinnDesktopTool.Utils.Helpers
{
    public class ProxyConfigHelper
    {
        public static IRestQueryConfig GetConfig()
        {
            return EnvironmentConfigurationManager.EnvironmentConfigurations.FirstOrDefault(c => c.Name =="PROD");
        }
    }
}
