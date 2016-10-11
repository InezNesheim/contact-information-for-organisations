using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class Links
    {
        public Link self { get; set; }
        public Link personalcontacts { get; set; }
        public Link officialcontacts { get; set; }
        public Link roles { get; set; }
    }
}
