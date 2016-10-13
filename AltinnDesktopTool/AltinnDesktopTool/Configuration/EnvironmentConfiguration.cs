﻿using RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltinnDesktopTool.Configuration
{
    public class EnvironmentConfiguration : IUIEnvironmentConfig, IRestQueryConfig    
    {
        public string Name { get; set; }

        public string ThemeName { get; set; }

        public string ApiKey { get; set; }

        public string BaseAddress { get; set; }        

        public string ThumbPrint { get; set; }        

        public int Timeout { get; set; }

        
    }
}
