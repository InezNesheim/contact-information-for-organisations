using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class PersonalContact
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
