using System.Collections.Generic;
using AltinnDesktopTool.Model;

using RestClient;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : ViewModelBase
    {
        private readonly ILog _logger;

        public SearchOrganizationInformationModel Model { get; set; }

        public RelayCommand<SearchOrganizationInformationModel> SearchCommand { get; set; }

        public IRestQuery RestProxy { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchOrganizationInformationModel();
            SearchCommand = new RelayCommand<SearchOrganizationInformationModel>(SearchOrganizations);
            RestProxy = new RestQueryStub();

            // Test loggers
            _logger.Debug("Debug!");
            _logger.Error("Error!");
            _logger.Warn("Warn!");
            _logger.Info("Info!");
        }

        private void SearchOrganizations(SearchOrganizationInformationModel obj)
        {
            _logger.Debug(GetType().FullName + " Seraching for: " + obj.SearchText + ", " + obj.SearchType);

            IList<Organization> organizations = null;

            switch (obj.SearchType)
            {
                case SearchType.EmailAddress:
                    {
                        organizations = RestProxy.Get<Organization>(new KeyValuePair<string, string>("email", obj.SearchText));
                        break;
                    }
                case SearchType.PhoneNumber:
                    {
                        organizations = RestProxy.Get<Organization>(new KeyValuePair<string, string>("phoneNumber", obj.SearchText));
                        break;
                    }
                case SearchType.OrganizationNumber:
                    {
                        var organization = RestProxy.Get<Organization>(obj.SearchText);
                        organizations = new List<Organization> {organization};
                        break;
                    }
            }

            // TODO: The application should have its own Model and not use the DTO
            MessengerInstance.Send(organizations);
        }
    }
}
