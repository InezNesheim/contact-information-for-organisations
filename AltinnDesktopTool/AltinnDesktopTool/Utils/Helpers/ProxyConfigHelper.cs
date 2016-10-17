using System;

using AltinnDesktopTool.Configuration;

using RestClient;

namespace AltinnDesktopTool.Utils.Helpers
{
    public class ProxyConfigHelper
    {
        public static IRestQueryConfig GetConfig()
        {
            return EnvironmentConfigurationManager.EnvironmentConfigurations[0];
        }
    }
}
