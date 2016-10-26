using System.Collections.ObjectModel;

namespace AltinnDesktopTool.Model
{
    /// <summary>
    /// Search Result Model
    /// </summary>
    public class SearchResultModel : ModelBase
    {
        private ObservableCollection<OrganizationModel> resultCollection;

        private bool emptyMessageVisibility;

        private string infoText = string.Empty;

        public bool EmptyMessageVisibility
        {
            get { return this.emptyMessageVisibility; }
            set
            {
                this.emptyMessageVisibility = value;
                this.RaisePropertyChanged(() => this.EmptyMessageVisibility);
                this.InfoText = value ? View.Resources.NoDataText : "";
            }
        }

        public string InfoText
        {
            get
            {
                return this.infoText;
            }
            set
            {
                this.infoText = value;
                this.RaisePropertyChanged(() => this.InfoText);
            }
        }

        public ObservableCollection<OrganizationModel> ResultCollection
        {
            get { return this.resultCollection; }
            set
            {
                this.resultCollection = value;
                this.RaisePropertyChanged(() => this.ResultCollection);
                this.EmptyMessageVisibility = this.resultCollection == null || this.resultCollection.Count == 0;
            }
        }
    }
}