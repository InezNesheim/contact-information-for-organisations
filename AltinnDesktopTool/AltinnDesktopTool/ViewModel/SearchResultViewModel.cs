using System.Collections.Generic;
using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;
using log4net;
using AltinnDesktopTool.Model;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using AutoMapper;
using RestClient;
using RestClient.Resources;
using System;

using AltinnDesktopTool.Utils.Helpers;

namespace AltinnDesktopTool.ViewModel
{
    public sealed class SearchResultViewModel : AltinnViewModelBase
    {
        private readonly ILog logger;
        private readonly IMapper mapper;
        private HashSet<OrganizationModel> organizationModels = new HashSet<OrganizationModel>();
        private IRestQuery restQuery;

        public new SearchResultModel Model { get; set; }

        public RelayCommand<OrganizationModel> GetContactsCommand { get; set; }
        public RelayCommand<OrganizationModel> ItemChecked { get; set; }
        public RelayCommand<OrganizationModel> ItemUnchecked { get; set; }
        public ICommand CopyToClipboardPlainTextCommand { get; private set; }
        public ICommand CopyToClipboardSemiColonSeparatedCommand { get; private set; }
        public bool Expanded { get; set; }

        //public event PubSubEventHandler<string> EnvironmentChangedEventHandler;


        public SearchResultViewModel(ILog logger, IMapper mapper, IRestQuery restQuery)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.restQuery = restQuery;

            this.Model = new SearchResultModel();

            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, this.SearchResultRecievedEventHandler);
            PubSub<bool>.RegisterEvent(EventNames.SearchStartedEvent, this.SearchStartedEventHandler);
            PubSub<string>.RegisterEvent(EventNames.EnvironmentChangedEvent, this.EnvironmentChangedEventHandler);


            this.GetContactsCommand = new RelayCommand<OrganizationModel>(this.GetContactsCommandHandler);
            this.ItemChecked = new RelayCommand<OrganizationModel>(this.ItemCheckedHandler);
            this.ItemUnchecked = new RelayCommand<OrganizationModel>(this.ItemUncheckedHandler);
            this.CopyToClipboardPlainTextCommand = new RelayCommand(this.CopyToClipboardPlainTextHandler);
            this.CopyToClipboardSemiColonSeparatedCommand = new RelayCommand(this.CopyToClipboardSemiColonSeparatedHandler);
            Expanded = true;
        }

        private void EnvironmentChangedEventHandler(object sender, PubSubEventArgs<string> e)
        {
            this.logger.Debug("Handling environment changed received event.");            
            this.restQuery = new RestQuery(ProxyConfigHelper.GetConfig(e.Item), this.logger);
        }

        private void SearchStartedEventHandler(object sender, PubSubEventArgs<bool> e)
        {
            this.Model.IsBusy = true;
            this.Model.ShowNoResultText = false;
        }

        private void GetContactsCommandHandler(OrganizationModel obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.OfficalContactsCollection == null && !string.IsNullOrEmpty(obj.OfficialContacts))
            {
                IList<OfficialContact> officialContactDtoCollection = new List<OfficialContact>();
                try
                {
                    officialContactDtoCollection = this.restQuery.GetByLink<OfficialContact>(obj.OfficialContacts);
                }
                catch (RestClientException rex)
                {
                    this.logger.Error("Exception from the RestClient", rex);
                }

                obj.OfficalContactsCollection =
                    this.mapper.Map<ICollection<OfficialContact>, ObservableCollection<OfficialContactModel>>(
                        officialContactDtoCollection);
            }

            if (obj.PersonalContactsCollection != null || string.IsNullOrEmpty(obj.PersonalContacts))
            {
                return;
            }

            IList<PersonalContact> personalContactDtoCollecton = new List<PersonalContact>();
            try
            {
                personalContactDtoCollecton = this.restQuery.GetByLink<PersonalContact>(obj.PersonalContacts);
            }
            catch (RestClientException rex)
            {
                this.logger.Error("Exception from the RestClient", rex);
            }

            obj.PersonalContactsCollection =
                this.mapper.Map<ICollection<PersonalContact>, ObservableCollection<PersonalContactModel>>(
                    personalContactDtoCollecton);
        }




        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            this.logger.Debug("Handling search result received event.");
            this.Model.ResultCollection = args.Item;
            if (args.Item == null || args.Item.Count == 0) this.Model.ShowNoResultText = true;
            else
            {
                this.Model.ShowNoResultText = false;
            }
            this.Model.IsBusy = false;
        }

        public void ItemCheckedHandler(OrganizationModel organizationModel)
        {
            this.GetContactsCommandHandler(organizationModel);
            this.organizationModels.Add(organizationModel);
        }

        public void ItemUncheckedHandler(OrganizationModel organizationModel)
        {
            this.GetContactsCommandHandler(organizationModel);
            this.organizationModels.Remove(organizationModel);
        }

        public void CopyToClipboardPlainTextHandler()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (OrganizationModel organizationModel in this.organizationModels)
            {
                stringBuilder.AppendFormat(organizationModel.Name + " " + organizationModel.OrganizationNumber + " " + organizationModel.Type + "{0}", Environment.NewLine);

                if (organizationModel.OfficalContactsCollection.Count > 0)
                {
                    stringBuilder.AppendFormat("Offisielle kontakter:{0}", Environment.NewLine);

                    foreach (OfficialContactModel officialContactModel in organizationModel.OfficalContactsCollection)
                    {
                        if (!String.IsNullOrEmpty(officialContactModel.EmailAddress) && !String.IsNullOrEmpty(officialContactModel.MobileNumber))
                        {
                            stringBuilder.AppendFormat(officialContactModel.EmailAddress + " " + officialContactModel.MobileNumber + "{0}", Environment.NewLine);
                            continue;
                        }

                        if (!String.IsNullOrEmpty(officialContactModel.EmailAddress))
                        {
                            stringBuilder.AppendFormat(officialContactModel.EmailAddress + "{0}", Environment.NewLine);
                            continue;
                        }

                        if (!String.IsNullOrEmpty(officialContactModel.MobileNumber))
                        {
                            stringBuilder.AppendFormat(officialContactModel.MobileNumber + "{0}", Environment.NewLine);
                        }
                    }
                }
                stringBuilder.Append(Environment.NewLine);
            }
            Clipboard.SetText(stringBuilder.ToString());
        }

        public void CopyToClipboardSemiColonSeparatedHandler()
        {

        }
    }
}
