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
    using System;

    public sealed class SearchOrganizationInformationViewModel : AltinnViewModelBase
    {
        private readonly ILog logger;
        private readonly IMapper mapper;
        private readonly IRestQuery query;

        public event PubSubEventHandler<ObservableCollection<OrganizationModel>> SearchResultRecievedEventHandler;

        public RelayCommand<SearchOrganizationInformationModel> SearchCommand { get; set; }

        public SearchOrganizationInformationViewModel(ILog logger, IMapper mapper, IRestQuery query)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.query = query;

            this.Model = new SearchOrganizationInformationModel();
            this.SearchCommand = new RelayCommand<SearchOrganizationInformationModel>(this.SearchCommandHandler);

            PubSub<ObservableCollection<OrganizationModel>>.AddEvent(EventNames.SearchResultRecievedEvent, this.SearchResultRecievedEventHandler);

            // Test loggers
            this.logger.Debug("Debug!");
            this.logger.Error("Error!");
            this.logger.Warn("Warn!");
            this.logger.Info("Info!");
        }

        private void SearchCommandHandler(SearchOrganizationInformationModel obj)
        {
            this.logger.Debug(this.GetType().FullName + " Searching for: " + obj.SearchText + ", " + obj.SearchType);
            
            IList<Organization> organizations = new List<Organization>();
            
            switch (obj.SearchType)
            {
                case SearchType.EmailAddress:
                {
                    organizations = this.query.Get<Organization>(new KeyValuePair<string, string>("email", obj.SearchText));
                    break;
                }
                case SearchType.PhoneNumber:
                {
                    organizations =
                    this.query.Get<Organization>(new KeyValuePair<string, string>("phoneNumber", obj.SearchText));
                    break;
                }
                case SearchType.OrganizationNumber:
                {
                    var organization = this.query.Get<Organization>(obj.SearchText);
                    organizations.Add(organization);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var orgmodellist = organizations != null ?
                this.mapper.Map<ICollection<Organization>, ObservableCollection<OrganizationModel>>(organizations) :
                new ObservableCollection<OrganizationModel>();

            PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this,
                new PubSubEventArgs<ObservableCollection<OrganizationModel>>(orgmodellist));
        }
    }
}
