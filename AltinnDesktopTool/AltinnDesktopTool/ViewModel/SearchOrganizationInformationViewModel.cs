using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.Helpers;
using AltinnDesktopTool.Utils.PubSub;

using AutoMapper;

using GalaSoft.MvvmLight.Command;

using log4net;

using RestClient;
using RestClient.DTO;
using RestClient.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using AltinnDesktopTool.View;

namespace AltinnDesktopTool.ViewModel
{
    /// <summary>
    /// ViewModel for SearchOrganizationInformation view
    /// </summary>    
    public sealed class SearchOrganizationInformationViewModel : AltinnViewModelBase
    {
        private readonly ILog logger;
        private readonly IMapper mapper;
        private IRestQuery query;

        public event PubSubEventHandler<bool> SearchStartedEventHandler;
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
            PubSub<bool>.AddEvent(EventNames.SearchStartedEvent, this.SearchStartedEventHandler);
            PubSub<string>.RegisterEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);

            // Test loggers
            //this.logger.Debug("Debug!");
            //this.logger.Error("Error!");
            //this.logger.Warn("Warn!");
            //this.logger.Info("Info!");
        }

        private async void SearchCommandHandler(SearchOrganizationInformationModel obj)
        {
            this.logger.Debug(this.GetType().FullName + " Searching for: " + obj.SearchText + ", " + obj.SearchType);

            obj.LabelText = string.Empty;
            obj.LabelBrush = Brushes.Green;

            // Removing all whitespaces from the search string.
            string searchText = new string(obj.SearchText.Where(c => !char.IsWhiteSpace(c)).ToArray());

            if (string.IsNullOrEmpty(searchText))
            {
                obj.LabelText = Resources.SearchLabelEmptySearch;
                obj.LabelBrush = Brushes.Red;

                // Preventing an empty search. It takes a lot of time and the result is useless. 
                return;
            }

            PubSub<bool>.RaiseEvent(EventNames.SearchStartedEvent, this, new PubSubEventArgs<bool>(true));

            // After having removed the radio buttons where the user could select search type, search is always Smart, but the check
            // is kept in case the radio buttons comes back in a future release. For example as advanced search.
            SearchType searchType = obj.SearchType == SearchType.Smart ? IdentifySearchType(searchText) : obj.SearchType;

            IList<Organization> organizations = new List<Organization>();

            try
            {
                switch (searchType)
                {
                    case SearchType.EmailAddress:
                    {
                        obj.LabelText = string.Format(Resources.SearchLabelResultat, Resources.EMail + " " + searchText);
                        organizations = await this.GetOrganizations("email", searchText);
                        break;
                    }
                    case SearchType.PhoneNumber:
                    {
                        obj.LabelText = string.Format(Resources.SearchLabelResultat, Resources.PhoneNumber + " " + searchText);
                        organizations = await this.GetOrganizations("", searchText);
                        break;
                    }
                    case SearchType.OrganizationNumber:
                    {
                        obj.LabelText = string.Format(Resources.SearchLabelResultat, Resources.OrganizationNumber + " " + searchText);
                        Organization organization = await this.GetOrganizations(searchText);
                        organizations.Add(organization);
                        break;
                    }
                    case SearchType.Smart:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (RestClientException rex)
            {
                obj.LabelText = Resources.SearchLabelErrorSearch;
                obj.LabelBrush = Brushes.Red;

                this.logger.Error("Exception from the RestClient", rex);
            }

                    ObservableCollection<OrganizationModel> orgmodellist = organizations != null
                        ? this.mapper.Map<ICollection<Organization>, ObservableCollection<OrganizationModel>>(organizations)
                        : new ObservableCollection<OrganizationModel>();
                    
            PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(
                EventNames.SearchResultRecievedEvent, this, new PubSubEventArgs<ObservableCollection<OrganizationModel>>(orgmodellist));
        }

        private async Task<Organization> GetOrganizations(string searchText)
        {
            return await Task.Run(() => this.query.Get<Organization>(searchText));
        }

        private async Task<IList<Organization>> GetOrganizations(string type, string searchText)
        {
            return await Task.Run(() => this.query.Get<Organization>(new KeyValuePair<string, string>(type, searchText)));
        }

        private static SearchType IdentifySearchType(string searchText)
        {
            if (searchText.IndexOf("@", StringComparison.InvariantCulture) > 0)
            {
                return SearchType.EmailAddress;
            }

            if (searchText.Length == 9 && searchText.All(char.IsDigit))
            {
                return SearchType.OrganizationNumber;
            }

            return SearchType.PhoneNumber;
        }

        public void EnvironmentChangedEventHandler(object sender, PubSubEventArgs<string> args)
        {
            this.logger.Debug("Handling environment changed received event.");
            IRestQueryConfig newConfig = ProxyConfigHelper.GetConfig(args.Item);

            this.query = new RestQuery(newConfig, this.logger);                                
            
            PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this,
               new PubSubEventArgs<ObservableCollection<OrganizationModel>>(new ObservableCollection<OrganizationModel>()));
        }
    }
}
