﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using AltinnDesktopTool.Configuration;
using AltinnDesktopTool.Model;
using AltinnDesktopTool.Utils.Helpers;
using AltinnDesktopTool.Utils.PubSub;

using AutoMapper;

using GalaSoft.MvvmLight.Command;

using log4net;

using MahApps.Metro;

using RestClient;
using RestClient.DTO;
using RestClient.Resources;

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
            PubSub<string>.RegisterEvent(EventNames.EnvironmentChangedEvent, EnvironmentChangedEventHandler);

            // Test loggers
            this.logger.Debug("Debug!");
            this.logger.Error("Error!");
            this.logger.Warn("Warn!");
            this.logger.Info("Info!");
        }

        private async void SearchCommandHandler(SearchOrganizationInformationModel obj)
        {
            this.logger.Debug(this.GetType().FullName + " Searching for: " + obj.SearchText + ", " + obj.SearchType);

            // Removing all whitespaces from the search string.
            string searchText = new string(obj.SearchText.Where(c => !char.IsWhiteSpace(c)).ToArray());

            if (string.IsNullOrEmpty(searchText))
            {
                // Preventing an empty search. It takes a lot of time and the result is useless. 
                return;
            }

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
                        organizations = await this.GetOrganizations("email", searchText);
                        break;
                    }
                    case SearchType.PhoneNumber:
                    {
                        organizations = await this.GetOrganizations("", searchText);
                        break;
                    }
                    case SearchType.OrganizationNumber:
                    {
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
                this.logger.Error("Exception from the RestClient", rex);
            }

            App.Current.Dispatcher.Invoke(
                () =>
                {
                    ObservableCollection<OrganizationModel> orgmodellist = organizations != null
                        ? this.mapper.Map<ICollection<Organization>, ObservableCollection<OrganizationModel>>(organizations)
                        : new ObservableCollection<OrganizationModel>();
                    PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this,
                                                                               new PubSubEventArgs
                                                                                   <ObservableCollection<OrganizationModel>>
                                                                                   (
                                                                                       orgmodellist));
                    
                });
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
            var newConfig = ProxyConfigHelper.GetConfig(args.Item);
            this.query = new RestQuery(newConfig, this.logger);

            //chnage theme
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent((newConfig as EnvironmentConfiguration).ThemeName), ThemeManager.GetAppTheme("BaseLight"));

            //clear result view
            PubSub<ObservableCollection<OrganizationModel>>.RaiseEvent(EventNames.SearchResultRecievedEvent, this,
               new PubSubEventArgs<ObservableCollection<OrganizationModel>>(new ObservableCollection<OrganizationModel>()));
        }
    }
}
