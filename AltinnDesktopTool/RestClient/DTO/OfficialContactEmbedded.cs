using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class OfficialContactEmbedded
    {
        public ICollection<OfficialContact> officialcontacts { get; set; }
    }
}
