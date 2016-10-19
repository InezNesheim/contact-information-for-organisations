using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Resources;


namespace AltinnDesktopTool.Model
{
    public class SearchResultModel : ModelBase
    {
        private ObservableCollection<OrganizationModel> _resultCollection;

        private bool emptyMessageVisibility = false;

        private string infoText = String.Empty;

        private bool initRun = true;

        public bool EmptyMessageVisibility
        {
            get { return emptyMessageVisibility; }
            set
            {
                emptyMessageVisibility = value;
                RaisePropertyChanged(() => EmptyMessageVisibility);
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
                RaisePropertyChanged(() => InfoText);
            }
        }

        public ObservableCollection<OrganizationModel> ResultCollection
        {
          get { return _resultCollection; }
          set
            {
                _resultCollection = value;
                RaisePropertyChanged(() => ResultCollection);
                if (!initRun)
                {
                    EmptyMessageVisibility = _resultCollection == null || _resultCollection.Count == 0;
                }
                else
                {
                    initRun = false;
                }
            }
        }

        public SearchResultModel()
        {
            ResultCollection = new ObservableCollection<OrganizationModel>();
        }

        
        
    }
}
