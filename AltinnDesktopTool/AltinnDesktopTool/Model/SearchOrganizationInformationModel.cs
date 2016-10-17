using System.ComponentModel.DataAnnotations;

namespace AltinnDesktopTool.Model
{
    public class SearchOrganizationInformationModel : ModelBase
    {
        private string _searchText;

        [Required(ErrorMessage = "You must enter a string.")]
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                //RaisePropertyChanged(() => SearchText);         
                RaisePropertyChanged("SearchText");
                ValidateModelProperty(value, "SearchText");
            }
        }

        public SearchType SearchType { get; set; }
    }
}