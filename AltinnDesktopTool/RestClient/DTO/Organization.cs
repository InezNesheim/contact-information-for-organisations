using System;

namespace RestClient.DTO
{
    [PluralName("Organizations")]
    public class Organization : HalJsonResource
    {
        public string Name { get; set; }
        public string OrganizationNumber { get; set; }
        public string Type { get; set; }
        public DateTime? LastChanged { get; set; }
        public DateTime? LastConfirmed { get; set; }

        public string OfficialContacts { get; set; }

        public string PersonalContacts { get; set; }
    }
}
