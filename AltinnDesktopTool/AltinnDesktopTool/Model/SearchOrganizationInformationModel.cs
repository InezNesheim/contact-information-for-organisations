using System.ComponentModel.DataAnnotations;

namespace AltinnDesktopTool.Model
{
    using AltinnDesktopTool.Properties;

    /// <summary>
    /// Search Model
    /// </summary>
    public class SearchOrganizationInformationModel : ModelBase
    {
        private string searchText;

        /// <summary>
        /// The Search Text as entered by the user
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SearchOrganizationInformationModel_SearchText_You_must_enter_a_string_")]
        public string SearchText
        {
            get { return this.searchText; }
            set
            {
                this.searchText = value;
                this.RaisePropertyChanged(() => this.SearchText);         
                //this.RaisePropertyChanged("SearchText");
                this.ValidateModelProperty(value, "SearchText");
            }
        }

        /// <summary>
        /// Indicates type of input
        /// </summary>
        public SearchType SearchType { get; set; }
    }
}