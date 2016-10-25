using System.Collections.ObjectModel;
using System.Threading;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.Helpers;
using AltinnDesktopTool.Utils.PubSub;
using AltinnDesktopTool.ViewModel;

using AutoMapper;

using log4net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RestClient;
using RestClient.DTO;

namespace AltinnDesktopToolTest.ViewModel
{
    /// <summary>
    /// Test class for unit tests of the <see cref="SearchOrganizationInformationViewModel"/> class.
    /// </summary>
    [TestClass]
    public class SearchOrganizationInformationViewModelTest
    {
        private static IMapper mapper;

        private ObservableCollection<OrganizationModel> searchResult;

        /// <summary>
        /// Gets or sets the test context for the current test.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Initialize the test class. This ensures that <see cref="AutoMapper"/> has been properly configured for all test methods and
        /// that logic performs actual mapping instead of having the mapping mocked.
        /// </summary>
        /// <param name="context">The current <see cref="TestContext"/> for the test class.</param>
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            mapper = AutoMapperHelper.RunCreateMaps();
        }

        /// <summary>
        /// Perform clean up after every unit test by removing the test result.
        /// </summary>
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
            Mock<ILog> logger = new Mock<ILog>();

            //logger.Setup(l => l.Error("Error!"));
            //logger.Setup(l => l.Warn("Warn!"));
            //logger.Setup(l => l.Info("Info!"));
            //logger.Setup(l => l.Debug("Debug!"));

            Mock<IRestQuery> query = new Mock<IRestQuery>();

            // Act
            SearchOrganizationInformationViewModel target = new SearchOrganizationInformationViewModel(logger.Object, mapper, query.Object);

            // Assert
            logger.VerifyAll();

            Assert.IsNotNull(target.Model);
            Assert.IsNotNull(target.SearchCommand);
        }

        #region Event tests

        /// <summary>
        /// Scenario: 
        ///   Search result is retrieved
        /// Expected Result: 
        ///   An event is published.
        /// Success Criteria: 
        ///   Event is retrieved in this test
        /// </summary>
        [TestMethod]
        [TestCategory("ViewModel")]
        public void SearchOrganizationInformationViewModelTest_SendsEventWhenSearchResultIsRecieved()
        {
            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, this.SearchResultRecievedEventHandler);

            SearchOrganizationInformationModel search = new SearchOrganizationInformationModel
            {
                SearchType = SearchType.OrganizationNumber,
                SearchText = "910021451"
            };

            SearchOrganizationInformationViewModel target = GetViewModel();

            target.SearchCommand.Execute(search);

            // Let  the other thread catch up.. 
            Thread.Sleep(1000);
            Assert.IsNotNull(this.searchResult);
        }

        #endregion

        #region Private Methods

        private static SearchOrganizationInformationViewModel GetViewModel()
        {
            Mock<ILog> logger = new Mock<ILog>();
            logger.Setup(l => l.Error("Error!"));
            logger.Setup(l => l.Warn("Warn!"));
            logger.Setup(l => l.Info("Info!"));
            logger.Setup(l => l.Debug("Debug!"));

            Organization org = new Organization();

            Mock<IRestQuery> query = new Mock<IRestQuery>();
            query.Setup(s => s.Get<Organization>(It.IsAny<string>())).Returns(org);

            SearchOrganizationInformationViewModel target = new SearchOrganizationInformationViewModel(logger.Object, mapper, query.Object);

            return target;
        }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            this.searchResult = args.Item;
        }

        #endregion
    }
}
