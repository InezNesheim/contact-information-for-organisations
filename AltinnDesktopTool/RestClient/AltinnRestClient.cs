using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace RestClient
{
    /// <summary>
    /// Generic Altinn Rest Client.
    /// </summary>
    public class AltinnRestClient : IDisposable
    {
        private HttpClient _httpClient;
        private string _baseAddress;
        private string _apikey;
        private int _timeout;
        private string _thumbprint;

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
        /// Timeout in Seconds for each request
        /// </summary>
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


        public AltinnRestClient(string baseAddress, string apiKey, string certificateThumbprint)
        {
            _baseAddress = baseAddress;
            _apikey = apiKey;
            _thumbprint = certificateThumbprint;
        }


        /// <summary>
        /// Performs a Get towards Altinn
        /// </summary>
        /// <param name="uriPart">The uriPart, added to base address if defined to form the full uri. If base address is undefined, this must be the full uri</param>
        /// <returns></returns>
        public string Get(string uriPart)
        {
            EnsureHandler();
            var responseMessage = _httpClient.GetAsync(uriPart, HttpCompletionOption.ResponseContentRead).Result;
            responseMessage.EnsureSuccessStatusCode();
            return responseMessage.Content.ReadAsStringAsync().Result;
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
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _apikey);
            if (_timeout > 0)
                _httpClient.Timeout = new TimeSpan(0, 0, 0, _timeout);
            if (!string.IsNullOrEmpty(_baseAddress))
                _httpClient.BaseAddress = new Uri(_baseAddress);
        }

    }

}
