using System;

namespace RestClient.DTO
{
    [PluralName("PersonalContacts")]
    public class PersonalContact : HalJsonResource
    {
        public string PersonalContactId { get; set; }
        public string Name { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? MobileNumberChanged { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? EmailAddressChanged { get; set; }
    }
}
