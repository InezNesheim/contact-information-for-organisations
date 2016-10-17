using System.Collections.Generic;

using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;

using GalaSoft.MvvmLight;
using log4net;
using AltinnDesktopTool.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using AutoMapper;
using RestClient;
using AltinnDesktopTool.Configuration;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IRestQuery _restQuery;

        public SearchResultViewModel(ILog logger, IMapper mapper, IRestQuery restQuery)
        {
            _logger = logger;
            _mapper = mapper;
            _restQuery = restQuery;
            
            Model = new SearchResultModel();

            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            GetContactsCommand = new RelayCommand<OrganizationModel>(GetContactsCommandHandler);
        }

        private void GetContactsCommandHandler(OrganizationModel obj)
        {
            if (obj != null)
            {
                if (obj.OfficalContactsCollection == null && !string.IsNullOrEmpty(obj.OfficialContacts))
                {
                    var officialContactDTOCollection = _restQuery.GetByLink<OfficialContact>(obj.OfficialContacts);
                    obj.OfficalContactsCollection = _mapper.Map<ICollection<OfficialContact>, ObservableCollection<OfficialContactModel>>(officialContactDTOCollection);
                }

                if (obj.PersonalContactsCollection == null && !string.IsNullOrEmpty(obj.PersonalContacts))
                {
                    var personalContactDTOCollecton = _restQuery.GetByLink<PersonalContact>(obj.PersonalContacts);
                    obj.PersonalContactsCollection = _mapper.Map<ICollection<PersonalContact>, ObservableCollection<PersonalContactModel>>(personalContactDTOCollecton);
                }
            }
        }

        public SearchResultModel Model { get; set; }

        public RelayCommand<OrganizationModel> GetContactsCommand { get; set; }

        public void SearchResultRecievedEventHandler(object sender, PubSubEventArgs<ObservableCollection<OrganizationModel>> args)
        {
            _logger.Debug("Handling search result received event.");

            Model.ResultCollection = args.Item;
        }
    }
}
