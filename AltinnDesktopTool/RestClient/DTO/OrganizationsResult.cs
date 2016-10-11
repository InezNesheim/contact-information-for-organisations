using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class OrganizationsResult
    {
        public Links _links { get; set; }
        public OrganizationEmbedded _embedded { get; set; }
    }
}
