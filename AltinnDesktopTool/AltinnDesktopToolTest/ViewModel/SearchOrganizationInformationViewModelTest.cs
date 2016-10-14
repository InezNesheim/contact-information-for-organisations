﻿using System.Collections.Generic;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.PubSub;
using AltinnDesktopTool.ViewModel;

using RestClient.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var target = new SearchOrganizationInformationViewModel(logger.Object, null);

            // Assert
            logger.VerifyAll();

            Assert.IsNotNull(target.Model);
            Assert.IsNotNull(target.SearchCommand);
        }

        #region Event tests

        private List<Organization> _searchResult = null;

        /// <summary>
        /// Scenario: 
        ///   Search result is recieved
        /// Expected Result: 
        ///   An event is published.
        /// Success Criteria: 
        ///   Event is recieved in this test
        /// </summary>
        [TestMethod]
        [TestCategory("ViewModel")]
        public void SearchOrganizationInformationViewModelTest_SendsEvent_WhenSearchResultIsRecieved()
        {
            PubSub<IList<Organization>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            var target = GetViewModel();

            target.SearchCommand.Execute(new SearchOrganizationInformationModel());

            Assert.IsNotNull(_searchResult);

        }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<IList<Organization>> args)
        {
            _searchResult = args.Item as List<Organization>;
        }

        #endregion

        #region Private Methods

        private static SearchOrganizationInformationViewModel GetViewModel()
        {
            // Arrange
            var logger = new Mock<ILog>();

            logger.Setup(l => l.Error("Error!"));
            logger.Setup(l => l.Warn("Warn!"));
            logger.Setup(l => l.Info("Info!"));
            logger.Setup(l => l.Debug("Debug!"));

            var target = new SearchOrganizationInformationViewModel(logger.Object, null);
            return target;
        }

        #endregion
    }
}
