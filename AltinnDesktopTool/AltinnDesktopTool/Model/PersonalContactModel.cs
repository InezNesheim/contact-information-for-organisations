namespace AltinnDesktopTool.Model
{
    /// <summary>
    /// Model for Personal Contact
    /// </summary>
    public class PersonalContactModel : ModelBase
    {
        /// <summary>
        /// Contact Name (as mapped from DTO)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Social Security Number (personnummer) (as mapped from DTO)
        /// </summary>
        public string SocialSecurityNumber { get; set; }

        /// <summary>
        /// Mobile Number (as mapped from DTO)
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Email address (as mapped from DTO)
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
