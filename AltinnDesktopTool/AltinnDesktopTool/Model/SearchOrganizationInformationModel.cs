using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace AltinnDesktopTool.Model
{
    using AltinnDesktopTool.Properties;

    /// <summary>
    /// This model holds the data required to perform a search and details about the label that displays the result of the search.
    /// </summary>
    public class SearchOrganizationInformationModel : ModelBase
    {
        private string searchText;
        private string labelText;
        private Brush labelBrush;

        /// <summary>
        /// Gets or sets the text to be displayed as feedback from the search.
        /// </summary>
        public string LabelText
        {
            get { return this.labelText; }
            set
            {
                this.labelText = value;
                this.RaisePropertyChanged(() => this.LabelText);
            }
        }

        /// <summary>
        /// Gets or sets the brush used to define the look of the label foreground color.
        /// </summary>
        public Brush LabelBrush
        {
            get { return this.labelBrush; }
            set
            {
                this.labelBrush = value;
                this.RaisePropertyChanged(() => this.LabelBrush);
            }
        }

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