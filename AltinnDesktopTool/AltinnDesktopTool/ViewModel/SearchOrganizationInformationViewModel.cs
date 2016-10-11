using System;
using AltinnDesktopTool.Model;
using GalaSoft.MvvmLight;
using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : ViewModelBase
    {
        private readonly ILog _logger;
        public SearchOrganizationInformationModel Model { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchOrganizationInformationModel();

            _logger.Debug("YEAH!");
        }
    }
}
