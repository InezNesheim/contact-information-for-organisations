using System.Collections.Generic;
using System.Collections.ObjectModel;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.PubSub;

using RestClient;
using RestClient.DTO;
using GalaSoft.MvvmLight.Command;

using log4net;
using AutoMapper;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchOrganizationInformationViewModel : AltinnViewModelBase
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IRestQuery _query;

        public event PubSubEventHandler<ObservableCollection<OrganizationModel>> SearchResultRecievedEventHandler;

        public RelayCommand<SearchOrganizationInformationModel> SearchCommand { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger, IMapper mapper, IRestQuery query)
        {
            _logger = logger;
            _mapper = mapper;
            _query = query;

            Model = new SearchOrganizationInformationModel();
            SearchCommand = new RelayCommand<SearchOrganizationInformationModel>(SearchCommandHandler);

            PubSub<ObservableCollection<OrganizationModel>>.AddEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            // Test loggers
            _logger.Debug("Debug!");
            _logger.Error("Error!");
            _logger.Warn("Warn!");
            _logger.Info("Info!");
        }

        private void SearchCommandHandler(SearchOrganizationInformationModel obj)
        {
            _logger.Debug(GetType().FullName + " Searching for: " + obj.SearchText + ", " + obj.SearchType);
            
            IList<Organization> organizations = new List<Organization>();
            
            switch (obj.SearchType)
            {
                case SearchType.EmailAddress:
                {
                    organizations = _query.Get<Organization>(new KeyValuePair<string, string>("email", obj.SearchText));
                    break;
                }
                case SearchType.PhoneNumber:
                {
                    organizations =
                    _query.Get<Organization>(new KeyValuePair<string, string>("phoneNumber", obj.SearchText));
                    break;
                }
                case SearchType.OrganizationNumber:
                {
                    var organization = _query.Get<Organization>(obj.SearchText);
                    organizations.Add(organization);
                    break;
                }
            }

            var orgmodellist = organizations != null ?
                _mapper.Map<ICollection<Organization>, ObservableCollection<OrganizationModel>>(organizations) :
                new ObservableCollection<OrganizationModel>();

            PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this,
                new PubSubEventArgs<ObservableCollection<OrganizationModel>>(orgmodellist));
        }
    }
}
