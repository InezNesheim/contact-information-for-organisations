using System.Collections.Generic;

using AltinnDesktopTool.Model;

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

            // TODO: The application should have its own Model and not use the DTO
            MessengerInstance.Register<IList<Organization>>(this, DisplayResults);
        }

        private void DisplayResults(IList<Organization> obj)
        {
            // TODO: Implement a refresh of the visible grid.
        }
    }
}
