using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using RestClient.Resources;
using System.Collections.Generic;

namespace RestClient
{

    /// <summary>
    /// Generic Altinn Rest Client.
    /// </summary>
    /// <remarks>
    /// Used internally by RestClient and the Controllers to communicate with Altinn REST Server interface.
    /// Will only requst hal+json as response format from the Server.
    /// </remarks>
    public class AltinnRestClient : IDisposable
    {
        #region private declarations
        private const string ACCEPTED_TYPE = "application/hal+json";
        private HttpClient _httpClient;
        private string _baseAddress;
        private string _apikey;
        private int _timeout;
        private string _thumbprint;
        #endregion

        #region public properties

        /// <summary>
        /// The Base address must include the part of the path up to the first controller name.
        /// </summary>
        /// <remarks>
        /// When the url is like: http://host/x/y/organizations/orgno
        /// and organizations is the name of the controller, then the base address must be:
        /// http://host/x/y
        /// And without the ending /
        /// The BaseAddress may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string BaseAddress
        {
            get
            {
                return _baseAddress;
            }
            set
            {
                _baseAddress = value;
                InvalidateHandler();
            }
        }


        /// <summary>
        /// The ApiKey is mandatory and must be provided in every call to Altinn server.
        /// </summary>
        /// <remarks>
        /// The ApiKey may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string ApiKey
        {
            get
            {
                return _apikey;
            }
            set
            {
                _apikey = value;
                InvalidateHandler();
            }
        }


        /// <summary>
        /// Timeout in Seconds for each request. Not mandatory.
        /// </summary>
        /// <remarks>
        /// Timeout may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
                InvalidateHandler();
            }
        }


        /// <summary>
        /// The Thumbprint of the certificate required to authenticate as Service owner.
        /// </summary>
        /// <remarks>
        /// The Certificate with this Thumbprint must be installed on the client computer in current user certificate store.
        /// Thumbprint may be changed, in which case AltinnRestClient will reconnect to new host on next call.
        /// </remarks>
        public string Thumbprint
        {
            get
            {
                return _thumbprint;
            }
            set
            {
                _thumbprint = value;
                InvalidateHandler();
            }
        }

        #endregion


        #region constructors
        /// <summary>
        /// Constructor providing the required properties
        /// </summary>
        public AltinnRestClient(string baseAddress, string apiKey, string certificateThumbprint)
        {
            _baseAddress = baseAddress;
            _apikey = apiKey;
            _thumbprint = certificateThumbprint;
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
            EnsureHandler();
            var responseMessage = _httpClient.GetAsync(uriPart, HttpCompletionOption.ResponseContentRead).Result;
            responseMessage.EnsureSuccessStatusCode();
            if (IsJsonResult(responseMessage))
                return responseMessage.Content.ReadAsStringAsync().Result;
            else
                return null;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InvalidateHandler();
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region private implementation
        private void InvalidateHandler()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }


        private void EnsureHandler()
        {
            if (_httpClient != null)
                return;
            InitHttpClient();
        }

        private bool IsJsonResult(HttpResponseMessage responseMessage)
        {
            var conttype = responseMessage.Content.Headers.ContentType.ToString();
            return conttype.StartsWith(ACCEPTED_TYPE, StringComparison.InvariantCultureIgnoreCase);
        }


        private void InitHttpClient()
        {
            WebRequestHandler httpClientHandler = new WebRequestHandler();

            if (string.IsNullOrEmpty(_apikey))
            {
                throw new RestClientException("ApiKey is missing");
            }

            if (string.IsNullOrEmpty(_thumbprint))
            {
                throw new RestClientException("Certificate Thumbprint is missing");
            }

            var store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificateColl = store.Certificates.Find(X509FindType.FindByThumbprint, _thumbprint, false);
            if (certificateColl.Count < 0)
            {
                throw new RestClientException("Certificate not found.");
            }
            var cert = certificateColl[0];
            bool verify = cert.Verify();
            if (!verify)
            {
                throw new RestClientException("Certificate not valid");
            }

            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ClientCertificates.Add(cert);
            _httpClient = new HttpClient(httpClientHandler, true);

            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _httpClient.DefaultRequestHeaders.Add("Accept", ACCEPTED_TYPE);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _apikey);
            if (_timeout > 0)
                _httpClient.Timeout = new TimeSpan(0, 0, 0, _timeout);
            if (!string.IsNullOrEmpty(_baseAddress))
                _httpClient.BaseAddress = new Uri(_baseAddress);
        }
        #endregion

    }

}
