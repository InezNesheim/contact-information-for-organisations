using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using RestClient.Controllers;
using RestClient.DTO;


namespace RestClient
{

    public class RestQuery : IRestQuery
    {
        private const string _authenticateUri = "organizations?ForceEIAuthentication";

        private IRestQueryConfig _restQueryConfig;
        private ILog _log;
        private AltinnRestClient _restClient;
        private bool _isAuthenticated = false;

        private Dictionary<string, Type> _controllers = new Dictionary<string, Type>();


        public IRestQueryConfig Config
        {
            get
            {
                return _restQueryConfig;
            }
            set
            {
                _restQueryConfig = value;
                _restClient.BaseAddress = _restQueryConfig.BaseAddress;
                _restClient.ApiKey = _restQueryConfig.ApiKey;
                _restClient.Thumbprint = _restQueryConfig.ThumbPrint;
                if (_restQueryConfig.Timeout > 0)
                    _restClient.Timeout = _restQueryConfig.Timeout;
            }
        }

        public RestQuery(IRestQueryConfig restQueryConfig, ILog log)
        {
            _restQueryConfig = restQueryConfig;
            _log = log;
            _restClient = new AltinnRestClient(restQueryConfig.BaseAddress, restQueryConfig.ApiKey, restQueryConfig.ThumbPrint);
            if (restQueryConfig.Timeout > 0)
                _restClient.Timeout = restQueryConfig.Timeout;

            // Add Controllers - Expand this when new controllers are available
            // A future expansion: define attributes and identify controllers by attributes using reflection
            _controllers.Add("organizations", typeof(OrganizationController));
        }

        public T Get<T>(string id) where T : HalJsonResource
        {
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for type {0}", typeof(T)));
            controller.Context = GetControllerContext();
            return controller.Get<T>(id);
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for type {0}", typeof(T)));
            controller.Context = GetControllerContext();
            return controller.Get<T>(filter);
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            IRestQueryController controller = GetControllerByUrl(url);
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for type {0}", typeof(T)));
            controller.Context = GetControllerContext();
            return controller.GetByLink<T>(url);
        }

        #region private implementation

        /// <summary>
        /// Ensures that this client is authenticated at the server side.
        /// </summary>
        /// <remarks>
        /// This has to be performed by catching exceptions due to an error as server always returns 401 error.
        /// </remarks>
        private void EnsureAuthenticated()
        {
            if (!_isAuthenticated)
            {
                try
                {
                    _restClient.Get(_authenticateUri);
                }
                catch
                {
                }
                _isAuthenticated = true;
            }
        }

        private ControllerContext GetControllerContext()
        {
            return new ControllerContext()
            {
                Log = this._log,
                RestClient = this._restClient
            };
        }

        private IRestQueryController GetControllerByType(Type t)
        {
            foreach (var item in _controllers)
            {
                if (item.Value == t)
                {
                    return (IRestQueryController)Activator.CreateInstance(item.Value);
                }
            }
            return null;
        }

        private IRestQueryController GetControllerByUrl(string url)
        {
            string name = url;
            int index = url.First(x => x == '/' || x == '?');
            if (index > 0)
                name = url.Substring(0, index);

            foreach (var item in _controllers)
            {
                if (item.Key.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return (IRestQueryController)Activator.CreateInstance(item.Value);
                }
            }
            return null;
        }

        #endregion
    }

}
