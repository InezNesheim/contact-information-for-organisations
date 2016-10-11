using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class OfficialContactsResult
    {
        public Links _links { get; set; }
        public OfficialContactEmbedded _embedded { get; set; }
    }
}   
