using System.Collections.Generic;

using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using log4net;
using AltinnDesktopTool.Model;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;
        
        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchResultModel();

            PubSub<IList<Organization>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);
        }

        public SearchResultModel Model { get; set; }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<IList<Organization>> args)
        {
            _logger.Debug("Handling search result received event.");

            Model.ResultCollection = new System.Collections.ObjectModel.ObservableCollection<OrganizationModel>()
            {
                new OrganizationModel() { Name = "My organization", OrganizationNumber = "orgno" },
                new OrganizationModel() { Name = "Coca company", OrganizationNumber = "cola orgno" },
            };

            // TODO Add result list to model bound to view
        }
    }
}
