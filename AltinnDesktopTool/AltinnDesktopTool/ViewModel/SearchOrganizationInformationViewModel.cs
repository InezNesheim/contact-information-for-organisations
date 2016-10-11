using System;
using AltinnDesktopTool.Model;
using GalaSoft.MvvmLight;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : ViewModelBase
    {
        public SearchOrganizationInformationModel Model { get; set; }

        public SearchOrganizationInformationViewModel()
        {
            Model = new SearchOrganizationInformationModel();
        }
    }
}
