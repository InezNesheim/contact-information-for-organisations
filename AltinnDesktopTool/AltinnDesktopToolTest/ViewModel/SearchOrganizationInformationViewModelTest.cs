using System.Collections.ObjectModel;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.Helpers;
using AltinnDesktopTool.Utils.PubSub;
using AltinnDesktopTool.ViewModel;

using RestClient;
using RestClient.DTO;

using AutoMapper;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AltinnDesktopToolTest.ViewModel
{
    [TestClass]
    public class SearchOrganizationInformationViewModelTest
    {
        private static IMapper mapper;

        private ObservableCollection<OrganizationModel> searchResult;

        /// <summary>
        /// Gets or sets the test context for the current test.
        /// </summary>
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            mapper = AutoMapperHelper.RunCreateMaps();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            this.searchResult = null;
        }

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

            var query = new Mock<IRestQuery>();

            // Act
            var target = new SearchOrganizationInformationViewModel(logger.Object, mapper, query.Object);

            // Assert
            logger.VerifyAll();

            Assert.IsNotNull(target.Model);
            Assert.IsNotNull(target.SearchCommand);
        }

        #region Event tests

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
        public void SearchOrganizationInformationViewModelTest_SendsEventWhenSearchResultIsRecieved()
        {
            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, this.SearchResultRecievedEventHandler);

            var search = new SearchOrganizationInformationModel
            {
                SearchType = SearchType.OrganizationNumber,
                SearchText = "910021451"
            };

            var target = GetViewModel();

            target.SearchCommand.Execute(search);

            Assert.IsNotNull(this.searchResult);

        }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            this.searchResult = args.Item;
        }

        #endregion

        #region Private Methods

        private static SearchOrganizationInformationViewModel GetViewModel()
        {
            var logger = new Mock<ILog>();
            logger.Setup(l => l.Error("Error!"));
            logger.Setup(l => l.Warn("Warn!"));
            logger.Setup(l => l.Info("Info!"));
            logger.Setup(l => l.Debug("Debug!"));

            var org = new Organization();

            var query = new Mock<IRestQuery>();
            query.Setup(s => s.Get<Organization>(It.IsAny<string>())).Returns(org);

            var target = new SearchOrganizationInformationViewModel(logger.Object, mapper, query.Object);

            return target;
        }

        #endregion
    }
}
