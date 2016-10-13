namespace RestClient
{
    /// <summary>
    /// The Query configuration as required by the RestQuery component.
    /// </summary>
    public interface IRestQueryConfig
    {
        /// <summary>
        /// The Base address must include the part of the path up to the first controller name.
        /// </summary>
        /// <remarks>
        /// When the url is like: http://host/x/y/organizations/orgno
        /// and organizations is the name of the controller, then the base address must be:
        /// http://host/x/y
        /// And without the ending /
        /// </remarks>
        string BaseAddress { get; set; }

        /// <summary>
        /// The ApiKey is required.
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// RestQuery uses certificate for authentication. This is the ThumbPrint of the certificate.
        /// </summary>
        string ThumbPrint { get; set; }

        /// <summary>
        /// An optional timeout related to the server request
        /// </summary>
        int Timeout { get; set; }
    }
}
