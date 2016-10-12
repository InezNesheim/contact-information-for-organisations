using log4net;

namespace RestClient.Controllers
{
    public class ControllerContext
    {
        public ILog Log { get; set; }

        public AltinnRestClient RestClient { get; set; }

        public string ControllerBaseAddress { get; set; }
    }
}
