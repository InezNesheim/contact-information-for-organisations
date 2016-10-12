namespace RestClient
{
    public interface IRestQueryConfig
    {
        string BaseAddress { get; set; }
        string ApiKey { get; set; }
        string ThumbPrint { get; set; }
        int Timeout { get; set; }
    }
}
