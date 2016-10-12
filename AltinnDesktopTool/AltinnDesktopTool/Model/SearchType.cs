namespace AltinnDesktopTool.Model
{
    /// <summary>
    /// Indicates the type of input given in the search.
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// The search input is a phone number.
        /// </summary>
        PhoneNumber = 0,

        /// <summary>
        /// The search input is an email address.
        /// </summary>
        EmailAddress = 1,

        /// <summary>
        /// The search input is an organization number.
        /// </summary>
        OrganizationNumber = 2
    }
}