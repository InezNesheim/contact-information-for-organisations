using System.Collections.ObjectModel;

namespace AltinnDesktopTool.Model
{
    public class OrganizationModel : ModelBase
    {
        ObservableCollection<OfficialContactModel> _officalContactsCollection;

        ObservableCollection<PersonalContactModel> _personalContactsCollection;

        public string Name { get; set; }
        public string OrganizationNumber { get; set; }
        public string Type { get; set; }
        public string OfficialContacts { get; set; }
        public string PersonalContacts { get; set; }

        public ObservableCollection<OfficialContactModel> OfficalContactsCollection
        {
            get { return _officalContactsCollection; }
            set
            {
                _officalContactsCollection = value;
                RaisePropertyChanged("OfficalContactsCollection");
            }
        }

        public ObservableCollection<PersonalContactModel> PersonalContactsCollection
        {
            get { return _personalContactsCollection; }
            set
            {
                _personalContactsCollection = value;
                RaisePropertyChanged("PersonalContactsCollection");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {2} ({1})", Name, OrganizationNumber, Type);
        }
    }
}
