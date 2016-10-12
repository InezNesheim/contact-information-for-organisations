using System;

namespace RestClient.DTO
{
    [PluralName("OfficialContacts")]
    public class OfficialContact : HalJsonResource
    {
        public string MobileNumber { get; set; }
        public DateTime? MobileNumberChanged { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? EmailAddressChanged { get; set; }
    }
}
