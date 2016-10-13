using System.Collections.Generic;
using AltinnDesktopTool.Utils;
using AltinnDesktopTool.Utils.PubSub;
using GalaSoft.MvvmLight;
using log4net;
using RestClient.DTO;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;

        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;

            PubSub < List<object>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);
        }

        private void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<List<object>> args)
        {
            // TODO Add result list to model bound to view
        }
    }
}
