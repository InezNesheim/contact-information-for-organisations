using RestClient;

namespace AltinnDesktopTool.Configuration
{
    public class EnvironmentConfiguration : IUiEnvironmentConfig, IRestQueryConfig    
    {
        public string Name { get; set; }

        public string ThemeName { get; set; }

        public string ApiKey { get; set; }

        public string BaseAddress { get; set; }        

        public string ThumbPrint { get; set; }        

        public int Timeout { get; set; }
    }
}
