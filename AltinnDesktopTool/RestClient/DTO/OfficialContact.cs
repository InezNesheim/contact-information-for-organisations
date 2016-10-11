using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class OfficialContact
    {
        public string MobileNumber { get; set; }
        public DateTime? MobileNumberChanged { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? EmailAddressChanged { get; set; }
    }
}
