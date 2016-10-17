using Microsoft.VisualStudio.TestTools.UnitTesting;
using AltinnDesktopTool.Configuration;

namespace AltinnDesktopToolTest
{
    [TestClass]
    public class EnvironmentConfigurationTest
    {
        [TestMethod]
        public void EnvironmentConfigurationManager_Load_Test()
        {
            var configs = EnvironmentConfigurationManager.EnvironmentConfigurations;
            Assert.IsNotNull(configs);
            Assert.IsTrue(configs.Count > 0);
        }
    }
}
