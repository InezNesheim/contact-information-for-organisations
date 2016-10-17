using AltinnDesktopTool.Utils.PubSub;
using RestClient.DTO;
using log4net;
using AltinnDesktopTool.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace AltinnDesktopTool.ViewModel
{
    public class SearchResultViewModel : AltinnViewModelBase
    {
        private readonly ILog _logger;

        public SearchResultViewModel(ILog logger)
        {
            _logger = logger;
            Model = new SearchResultModel();

            PubSub<ObservableCollection<OrganizationModel>>.RegisterEvent(EventNames.SearchResultRecievedEvent, SearchResultRecievedEventHandler);

            GetContactsCommand = new RelayCommand<OrganizationModel>(GetContactsCommandHandler);
        }

        private void GetContactsCommandHandler(OrganizationModel obj)
        {
            // TODO get contacts 

            /*
             <i:Interaction.Triggers>
                <i:EventTrigger EventName="Click" >
                    <i:InvokeCommandAction Command="{Binding GetContactsCommand}" CommandParameter="{Binding Item}"/>
                </i:EventTrigger>
            </i:Interaction.Triggers>
             */
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
