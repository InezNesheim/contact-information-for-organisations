using System.Collections.ObjectModel;


namespace AltinnDesktopTool.Model
{
    public class SearchResultModel : ModelBase
    {
        private ObservableCollection<OrganizationModel> _resultCollection;

        public ObservableCollection<OrganizationModel> ResultCollection
        {
          get { return _resultCollection; }
          set
            {
                _resultCollection = value;
                RaisePropertyChanged(() => ResultCollection);
            }
        }

        public SearchResultModel()
        {
            ResultCollection = new ObservableCollection<OrganizationModel>();
        }
    }
}
