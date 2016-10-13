using RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltinnDesktopTool.Configuration
{
    public class EnvironmentConfiguration : IRestQueryConfig
    {
        public string Name { get; set; }

        public string ApiKey { get; set; }

        public string BaseAddress { get; set; }        

        public string ThumbPrint { get; set; }        

        public int Timeout { get; set; }       
    }
}
