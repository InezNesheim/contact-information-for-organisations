using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltinnDesktopTool.Model
{
    public class OrganizationModel : ModelBase
    {
        public string Name { get; set; }
        public string OrganizationNumber { get; set; }
        public string Type { get; set; }
        public string OfficialContacts { get; set; }
        public string PersonalContacts { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {2} ({1})", Name, OrganizationNumber, Type);
        }
    }
}
