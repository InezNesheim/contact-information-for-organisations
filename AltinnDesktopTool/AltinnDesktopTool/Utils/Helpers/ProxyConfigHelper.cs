using System.Linq;
using AltinnDesktopTool.Configuration;
using RestClient;

namespace AltinnDesktopTool.Utils.Helpers
{
    /// <summary>
    /// Helper class for the Proxy Configuration
    /// </summary>
    public class ProxyConfigHelper
    {
        /// <summary>
        /// Gets the RestQueryConfig object from the configuration manager
        /// </summary>
        /// <returns>The IRestQueryConfig object</returns>
        public static IRestQueryConfig GetConfig()
        {
            return EnvironmentConfigurationManager.EnvironmentConfigurations.FirstOrDefault(c => c.Name == "PROD");
        }
    }
}
