namespace RestClient.Controllers
{
    public interface IRestQueryController : IRestQuery
    {
        ControllerContext Context { get; set; }
    }
}
