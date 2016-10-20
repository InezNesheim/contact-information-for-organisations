namespace AltinnDesktopTool.Model
{
    /// <summary>
    /// Model for Official Contact
    /// </summary>
    public class OfficialContactModel : ModelBase
    {
        /// <summary>
        /// The Mobile number as recieved from source
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// The Email Address as received from source
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
