using System.Collections.Generic;

using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;

        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;

            PubSub<IList<Organization>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);
        }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<IList<Organization>> args)
        {
            _logger.Debug("Handling search result received event.");

            // TODO Add result list to model bound to view
        }
    }
}
