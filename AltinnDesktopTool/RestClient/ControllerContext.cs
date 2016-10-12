using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace RestClient
{
    public class ControllerContext
    {
        public ILog Log { get; set; }
        public AltinnRestClient RestClient { get; set; }        
    }
}
