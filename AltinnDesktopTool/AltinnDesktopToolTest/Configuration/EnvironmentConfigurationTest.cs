using AltinnDesktopTool.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AltinnDesktopToolTest.Configuration
{
    [TestClass]
    public class EnvironmentConfigurationTest
    {
        [TestMethod]
        public void EnvironmentConfigurationManagerLoadTest()
        {
            var configs = EnvironmentConfigurationManager.EnvironmentConfigurations;
            Assert.IsNotNull(configs);
            Assert.IsTrue(configs.Count > 0);
        }
    }
}
