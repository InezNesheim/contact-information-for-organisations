﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltinnDesktopTool.Configuration;

namespace AltinnDesktopToolTest
{
    [TestClass]
    public class EnvironmentConfigurationTest
    {
        [TestMethod]
        public void EnvironmentConfigurationManager_Load_Test()
        {
            var manager = new EnvironmentConfigurationManager();
            var configs = manager.EnvironmentConfigurations;
            Assert.IsNotNull(configs);
            Assert.IsTrue(configs.Count > 0);
        }
    }
}