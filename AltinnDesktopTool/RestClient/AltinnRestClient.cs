using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

using RestClient.Resources;

namespace RestClient
{
    /// <summary>
    /// Generic Altinn Rest Client.
    /// </summary>
    /// <remarks>
    /// Used internally by RestClient and the Controllers to communicate with Altinn REST Server interface.
    /// Will only requst "hal+json" as response format from the Server.
    /// </remarks>
    public class AltinnRestClient : IDisposable
    {
        #region private declarations

        private const string AcceptedType = "application/hal+json";
        private HttpClient httpClient;
        private string baseAddress;
        private string apikey;
        private int timeout;
        private string thumbprint;

        #endregion

        #region public properties

        /// <summary>
        /// Gets or sets the base address of the API being used by this client.
        /// </summary>
        /// <remarks>
        /// When the url is like: "http://host/x/y/organizations/orgno"
        /// and organizations is the name of the controller, then the base address must be:
        /// http://host/x/y
        /// And without the ending /
        /// The BaseAddress may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string BaseAddress
        {
            get
            {
                return this.baseAddress;
            }

            set
            {
                this.baseAddress = value;
                this.InvalidateHandler();
            }
        }

        /// <summary>
        /// Gets or sets the ApiKey to be used by the client. 
        /// </summary>
        /// <remarks>
        /// The ApiKey is a mandatory value to have in the request header when using the Altinn API.
        /// The ApiKey may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string ApiKey
        {
            get
            {
                return this.apikey;
            }

            set
            {
                this.apikey = value;
                this.InvalidateHandler();
            }
        }

        /// <summary>
        /// Gets or sets the timeout for a request in seconds.
        /// </summary>
        /// <remarks>
        ///  Not mandatory. Timeout may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public int Timeout
        {
            get
            {
                return this.timeout;
            }

            set
            {
                this.timeout = value;
                this.InvalidateHandler();
            }
        }

        /// <summary>
        /// Gets or sets the thumbprint of the certificate required to authenticate as service owner.
        /// </summary>
        /// <remarks>
        /// The Certificate with this Thumbprint must be installed on the client computer in current user certificate store.
        /// Thumbprint may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string Thumbprint
        {
            get
            {
                return this.thumbprint;
            }

            set
            {
                this.thumbprint = value;
                this.InvalidateHandler();
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Constructor providing the required properties
        /// </summary>
        /// <param name="baseAddress">The base url for the API being used by the client.</param>
        /// <param name="apiKey">The ApiKey for the specific application using the client.</param>
        /// <param name="certificateThumbprint">The thumbprint for the enterprise certificate for the service owner.</param>
        public AltinnRestClient(string baseAddress, string apiKey, string certificateThumbprint)
        {
            this.baseAddress = baseAddress;
            this.apikey = apiKey;
            this.thumbprint = certificateThumbprint;
        }

        #endregion
    
        #region public and protected methods

        /// <summary>
        /// Performs a Get towards Altinn
        /// </summary>
        /// <param name="uriPart">The uriPart, added to base address if defined to form the full uri. If base address is undefined, this must be the full uri</param>
        /// <returns>hal+Json data string or null if not found</returns>
        /// <remarks>
        /// Exception is raised on communication error or error returned from Altinn server.
        /// </remarks>
        public string Get(string uriPart)
        {
            this.EnsureHandler();
            HttpResponseMessage responseMessage = this.httpClient.GetAsync(uriPart, HttpCompletionOption.ResponseContentRead).Result;
            responseMessage.EnsureSuccessStatusCode();

            return IsJsonResult(responseMessage) ? responseMessage.Content.ReadAsStringAsync().Result : null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.InvalidateHandler();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region private implementation
        private void InvalidateHandler()
        {
            if (this.httpClient != null)
            {
                this.httpClient.Dispose();
                this.httpClient = null;
            }
        }

        private void EnsureHandler()
        {
            if (this.httpClient != null)
            {
                return;
            }

            this.InitHttpClient();
        }

        private static bool IsJsonResult(HttpResponseMessage responseMessage)
        {
            string conttype = responseMessage.Content.Headers.ContentType.ToString();
            return conttype.StartsWith(AcceptedType, StringComparison.InvariantCultureIgnoreCase);
        }

        private void InitHttpClient()
        {
            WebRequestHandler httpClientHandler = new WebRequestHandler();

            if (string.IsNullOrEmpty(this.apikey))
            {
                throw new RestClientException("ApiKey is missing");
            }

            if (string.IsNullOrEmpty(this.thumbprint))
            {
                throw new RestClientException("Certificate Thumbprint is missing");
            }

            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certificateColl = store.Certificates.Find(X509FindType.FindByThumbprint, this.thumbprint, false);
            if (certificateColl.Count < 0)
            {
                throw new RestClientException("Certificate not found.");
            }

            X509Certificate2 cert = certificateColl[0];
            bool verify = cert.Verify();

            if (!verify)
            {
                throw new RestClientException("Certificate not valid");
            }

            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ClientCertificates.Add(cert);

            this.httpClient = new HttpClient(httpClientHandler, true);
            this.httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            this.httpClient.DefaultRequestHeaders.Add("Accept", AcceptedType);
            this.httpClient.DefaultRequestHeaders.Add("ApiKey", this.apikey);

            if (this.timeout > 0)
            {
                this.httpClient.Timeout = new TimeSpan(0, 0, 0, this.timeout);
            }

            if (!string.IsNullOrEmpty(this.baseAddress))
            {
                this.httpClient.BaseAddress = new Uri(this.baseAddress);
            }
        }

        #endregion
    }
}
