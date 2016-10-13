namespace RestClient.Controllers
{

    /// <summary>
    /// The controller interface. Inherits the IRestQuery by adding the ControllerContext.
    /// </summary>
    public interface IRestQueryController : IRestQuery
    {
        /// <summary>
        /// Contains the required data for the Controller.
        /// </summary>
        ControllerContext Context { get; set; }
    }
}
