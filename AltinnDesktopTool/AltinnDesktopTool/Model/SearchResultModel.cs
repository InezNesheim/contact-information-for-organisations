using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
