using log4net;

namespace RestClient.Controllers
{
    /// <summary>
    /// The required data for the controller.
    /// </summary>
    public class ControllerContext
    {
        /// <summary>
        /// The log4net Log object, may be null.
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// The Http wrapper for REST calls to Altinn server.
        /// </summary>
        public AltinnRestClient RestClient { get; set; }

        /// <summary>
        /// The base address for this controller including the controller name.
        /// </summary>
        public string ControllerBaseAddress { get; set; }
    }
}
