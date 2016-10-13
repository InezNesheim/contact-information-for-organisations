using System.Collections.Generic;
using AltinnDesktopTool.Model;

using RestClient;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using log4net;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : ViewModelBase
    {
        private readonly ILog _logger;

        public SearchOrganizationInformationModel Model { get; set; }

        public RelayCommand<SearchOrganizationInformationModel> SearchCommand { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchOrganizationInformationModel();
            SearchCommand = new RelayCommand<SearchOrganizationInformationModel>(SearchOrganizations);

            // Test loggers
            _logger.Debug("Debug!");
            _logger.Error("Error!");
            _logger.Warn("Warn!");
            _logger.Info("Info!");
        }

        private void SearchOrganizations(SearchOrganizationInformationModel obj)
        {
            _logger.Debug(GetType().FullName + " Seraching for: " + obj.SearchText + ", " + obj.SearchType);
            // TODO call proxy and get orgs

            IList<Organization> organizations = new List<Organization>();

            switch (obj.SearchType)
            {
                case SearchType.EmailAddress:
                    {
                        IRestQuery query = new RestQueryStub();
                        organizations = query.Get<Organization>(new KeyValuePair<string, string>("email", obj.SearchText));
                        break;
                    }
                case SearchType.PhoneNumber:
                    {
                        IRestQuery query = new RestQueryStub();
                        organizations = query.Get<Organization>(new KeyValuePair<string, string>("phonenumber", obj.SearchText));
                        break;
                    }
                case SearchType.OrganizationNumber:
                    {
                        IRestQuery query = new RestQueryStub();
                        var organization = query.Get<Organization>(obj.SearchText);
                        organizations.Add(organization);
                        break;
                    }
            }
        }
    }
}
