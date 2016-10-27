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
        /// Gets or sets the Organization Name (as mapped from DTO)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Organization Number (as mapped from DTO)
        /// </summary>
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// Gets or sets the Organization Type (AS, ANS etc) (as mapped from DTO)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Link to official contacts (as mapped from DTO)
        /// </summary>
        public string OfficialContacts { get; set; }

        /// <summary>
        /// Gets or sets the Link to personal contacts (as mapped from DTO)
        /// </summary>
        public string PersonalContacts { get; set; }

        /// <summary>
        /// Gets or sets the Collection of Official Contacts, populate this to automatically update view
        /// </summary>
        public ObservableCollection<OfficialContactModel> OfficalContactsCollection
        {
            get
            {
                return this.officalContactsCollection;
            }

            set
            {
                this.officalContactsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Collection of Personal Contacts, populate this to automatically update view
        /// </summary>
        public ObservableCollection<PersonalContactModel> PersonalContactsCollection
        {
            get
            {
                return this.personalContactsCollection;
            }

            set
            {
                this.personalContactsCollection = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
