using Microsoft.VisualStudio.TestTools.UnitTesting;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.ViewModel;

using log4net;
using Moq;

namespace AltinnDesktopToolTest.ViewModel
{
    [TestClass]
    public class SearchOrganizationInformationViewModelTest
    {
        /// <summary>
        /// Gets or sets the test context for the current test.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Scenario: 
        ///   Instantiate a new instance of SearchOrganizationInformationViewModel.
        /// Expected Result: 
        ///   A new instance of SearchOrganizationInformationViewModel is created.
        /// Success Criteria: 
        ///   The Model and SearchCommand properties are being populated and the logger is being called.
        /// </summary>
        [TestMethod]
        [TestCategory("ViewModel")]
        public void SearchOrganizationInformationViewModelTest_Instantiation()
        {
            // Arrange
            var logger = new Mock<ILog>();

            logger.Setup(l => l.Error("Error!"));
            logger.Setup(l => l.Warn("Warn!"));
            logger.Setup(l => l.Info("Info!"));
            logger.Setup(l => l.Debug("Debug!"));

            // Act
            var target = new SearchOrganizationInformationViewModel(logger.Object);

            // Assert
            logger.VerifyAll();

            Assert.IsNotNull(target.Model);
            Assert.IsNotNull(target.SearchCommand);
        }

        [TestMethod]
        [TestCategory("ViewModel")]
        public void SearchCommandTest_PerformSearchBasedOnOrganizationNumber()
        {
            // Arrange
            var logger = new Mock<ILog>();

            var search = new SearchOrganizationInformationModel();

            var target = new SearchOrganizationInformationViewModel(logger.Object);

            // Act
            target.SearchCommand.Execute(search);

            // Assert
            Assert.Inconclusive("Asserts needed!");
        }
    }
}
