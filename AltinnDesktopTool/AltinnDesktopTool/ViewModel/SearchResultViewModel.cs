using System.Collections.Generic;

using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using log4net;
using AltinnDesktopTool.Model;
using System.Collections.ObjectModel;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;
        
        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchResultModel();

            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);
        }

        public SearchResultModel Model { get; set; }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            _logger.Debug("Handling search result received event.");

            Model.ResultCollection = args.Item;

            // TODO Add result list to model bound to view
        }
    }
}
