namespace AltinnDesktopTool.Model
{
    public class SearchOrganizationInformationModel : ModelBase
    {
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                //RaisePropertyChanged(() => SearchText);         
                RaisePropertyChanged("SearchText");
            }
        }

        public SearchType SearchType { get; set; }
    }
}