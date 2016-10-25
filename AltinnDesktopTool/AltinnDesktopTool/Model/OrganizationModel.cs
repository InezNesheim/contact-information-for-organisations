using System.Collections.ObjectModel;

namespace AltinnDesktopTool.Model
{
    /// <summary>
    /// Model for Organization
    /// </summary>
    public class OrganizationModel : ModelBase
    {
        private ObservableCollection<OfficialContactModel> officalContactsCollection;

        private ObservableCollection<PersonalContactModel> personalContactsCollection;

        /// <summary>
        /// Organization Name (as mapped from DTO)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Organization Number (as mapped from DTO)
        /// </summary>
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// Organization Type (AS, ANS etc) (as mapped from DTO)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Link to official contacts (as mapped from DTO)
        /// </summary>
        public string OfficialContacts { get; set; }

        /// <summary>
        /// Link to personal contacts (as mapped from DTO)
        /// </summary>
        public string PersonalContacts { get; set; }

        /// <summary>
        /// Used to check whether an item is selected or not. Used to copy to clipboard.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Collection of Official Contacts, populate this to automatically update view
        /// </summary>
        public ObservableCollection<OfficialContactModel> OfficalContactsCollection
        {
            get { return this.officalContactsCollection; }
            set
            {
                this.officalContactsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Collection of Personal Contacts, populate this to automatically update view
        /// </summary>
        public ObservableCollection<PersonalContactModel> PersonalContactsCollection
        {
            get { return this.personalContactsCollection; }
            set
            {
                this.personalContactsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {2} ({1})", this.Name, this.OrganizationNumber, this.Type);
        }
    }
}
