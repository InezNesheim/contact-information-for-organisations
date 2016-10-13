using System.Collections.Generic;
using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.PubSub;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : ViewModelBase
    {
        public event PubSubEventHandler<List<object>> SearchResultRecievedEventHandler;

        private readonly ILog _logger;
        

        public SearchOrganizationInformationModel Model { get; set; }

        public RelayCommand<SearchOrganizationInformationModel> SearchCommand { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchOrganizationInformationModel();
            SearchCommand = new RelayCommand<SearchOrganizationInformationModel>(SearchCommandHandler);

            PubSub<List<object>>.AddEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            // Test loggers
            _logger.Debug("Debug!");
            _logger.Error("Error!");
            _logger.Warn("Warn!");
            _logger.Info("Info!");
        }

        private void SearchCommandHandler(SearchOrganizationInformationModel obj)
        {
            
            _logger.Debug(GetType().FullName + " Seraching for: " + obj.SearchText + ", " + obj.SearchType);
            // TODO call proxy and get orgs
            
            var result = new List<object>(); // TODO Change object to relevant model
            PubSub<List<object>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this, new PubSubEventArgs<List<object>>(result));
            return;
        }
    }
}
