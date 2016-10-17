using System;

namespace AltinnDesktopTool.Model
{
    public class OfficialContactModel : ModelBase
    {
        public string MobileNumber { get; set; }
        public DateTime? MobileNumberChanged { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? EmailAddressChanged { get; set; }
    }
}
