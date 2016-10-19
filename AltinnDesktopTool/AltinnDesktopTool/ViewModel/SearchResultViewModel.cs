using System.Collections.Generic;
using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;
using log4net;
using AltinnDesktopTool.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using AutoMapper;
using RestClient;

namespace AltinnDesktopTool.ViewModel
{
    public sealed class SearchResultViewModel : AltinnViewModelBase
    {
        private readonly ILog logger;
        private readonly IMapper mapper;
        private readonly IRestQuery restQuery;

        public SearchResultViewModel(ILog logger, IMapper mapper, IRestQuery restQuery)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.restQuery = restQuery;

            this.Model = new SearchResultModel();

            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            this.GetContactsCommand = new RelayCommand<OrganizationModel>(this.GetContactsCommandHandler);
        }

        private void GetContactsCommandHandler(OrganizationModel obj)
        {
            if (obj == null) return;
            if (obj.OfficalContactsCollection == null && !string.IsNullOrEmpty(obj.OfficialContacts))
            {
                var officialContactDtoCollection = this.restQuery.GetByLink<OfficialContact>(obj.OfficialContacts);
                obj.OfficalContactsCollection = this.mapper.Map<ICollection<OfficialContact>, ObservableCollection<OfficialContactModel>>(officialContactDtoCollection);
            }

            if (obj.PersonalContactsCollection != null || string.IsNullOrEmpty(obj.PersonalContacts)) return;
            var personalContactDtoCollecton = this.restQuery.GetByLink<PersonalContact>(obj.PersonalContacts);
            obj.PersonalContactsCollection = this.mapper.Map<ICollection<PersonalContact>, ObservableCollection<PersonalContactModel>>(personalContactDtoCollecton);
        }

        public new SearchResultModel Model { get; set; }

        public RelayCommand<OrganizationModel> GetContactsCommand { get; set; }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            this.logger.Debug("Handling search result received event.");
            this.Model.ResultCollection = args.Item;
        }
    }
}
