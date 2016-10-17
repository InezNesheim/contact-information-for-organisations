using System.Collections.ObjectModel;

namespace AltinnDesktopTool.Model
{
    public class OrganizationModel : ModelBase
    {
        public string Name { get; set; }
        public string OrganizationNumber { get; set; }
        public string Type { get; set; }
        public string OfficialContacts { get; set; }
        public string PersonalContacts { get; set; }

        public ObservableCollection<OfficialContactModel> OfficalContactsCollection { get; set; }

        public ObservableCollection<PersonalContactModel> PersonalContactsCollection { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {2} ({1})", Name, OrganizationNumber, Type);
        }
    }
}
