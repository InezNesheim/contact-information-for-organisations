using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class PersonalContactsResult
    {
        public Links _links { get; set; }
        public PersonalContactEmbedded _embedded { get; set; }
    }
}
