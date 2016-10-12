using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using RestClient.Controllers;
using RestClient.DTO;
using System.Reflection;

namespace RestClient
{

    /// <summary>
    /// The Rest Query class implements IRestQuery. It is a generic implementation passing queries to IRestQueryControllers to perform the actual query.
    /// </summary>
    public class RestQuery : IRestQuery
    {
        private const string _authenticateUri = "organizations?ForceEIAuthentication";

        private IRestQueryConfig _restQueryConfig;
        private ILog _log;
        private AltinnRestClient _restClient;
        private bool _isAuthenticated = false;
        private List<Controller> _controllers = new List<Controller>();

        /// <summary>
        /// Gets or sets the configuration. 
        /// The configuration may be changed in which it will reconnect according to new configuration at first request.
        /// </summary>
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


        /// <summary>
        /// Constructs the RestQuery by injecting the configuration and log.
        /// The configuration is mandatory, whereas the log is suggested, but not mandatory
        /// </summary>
        /// <param name="restQueryConfig">The configuration needed for connecting</param>
        /// <param name="log">Optional log4net log instance</param>
        public RestQuery(IRestQueryConfig restQueryConfig, ILog log = null)
        {
            _restQueryConfig = restQueryConfig;
            _log = log;
            _restClient = new AltinnRestClient(restQueryConfig.BaseAddress, restQueryConfig.ApiKey, restQueryConfig.ThumbPrint);
            if (restQueryConfig.Timeout > 0)
                _restClient.Timeout = restQueryConfig.Timeout;
            InitControllers();
        }


        /// <summary>
        /// Gets an object identified by id. Fetches the correct controller based on type.
        /// </summary>
        /// <typeparam name="T">The type of object to be retrieved</typeparam>
        /// <param name="id">A unique identifier for the object</param>
        /// <returns>The found object or null if not found</returns>
        public T Get<T>(string id)
        {
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for type {0}", typeof(T)));
            return controller.Get<T>(id);
        }


        /// <summary>
        /// Search for a list of objects by filtering on a given name value pair.
        /// The possible values name value pairs depends on the controller being called.
        /// The controller is identified by the type T.
        /// </summary>
        /// <typeparam name="T">The type of objects to be retrieved. This also determines the controller to call.</typeparam>
        /// <param name="filter">The name value pair filter</param>
        /// <returns>A list of objects, possibly empty, but never null.</returns>
        public IList<T> Get<T>(KeyValuePair<string, string> filter)
        {
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for type {0}", typeof(T)));
            return controller.Get<T>(filter);
        }


        /// <summary>
        /// Fetches a list of objects by a given link (url).
        /// This is useful where a link is returned in a previous call.
        /// </summary>
        /// <typeparam name="T">The type of object to be retrieved.</typeparam>
        /// <param name="url">The url, possibly including base address (full url).</param>
        /// <returns>A lif objects, possibly empty, but never null</returns>
        public IList<T> GetByLink<T>(string url)
        {
            IRestQueryController controller = GetControllerByUrl(url, typeof(T));
            if (controller == null)
                throw new RestClientException(string.Format("No Controller for url {0}", url));
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

        private ControllerContext GetControllerContext(Controller controller)
        {
            return new ControllerContext()
            {
                Log = this._log,
                RestClient = this._restClient,
                ControllerBaseAddress = string.Format("{0}/{1}", _restQueryConfig.BaseAddress, controller.Name)
            };
        }

        private IRestQueryController GetControllerByType(Type t)
        {
            IRestQueryController controller = null;
            foreach (var item in _controllers)
            {
                if (item.SupportedType == t)
                {
                    controller = (IRestQueryController)Activator.CreateInstance(item.ControllerType);
                    controller.Context = GetControllerContext(item);
                    break;
                }
            }
            return controller;
        }

        /// <summary>
        /// Fetches the controller by its URL and object type. The url controller part contains the name. 
        /// The controller part is the first word after the base address.
        /// It takes into account that the url may contain the base address.
        /// </summary>
        /// <param name="url">Either a full address inkcluding base address or a url part starting with controller name.</param>
        /// <returns>The controller, initiated with context</returns>
        private IRestQueryController GetControllerByUrl(string url, Type type)
        {
            IRestQueryController controller = null;
            string u = url;
            if (u.StartsWith(_restQueryConfig.BaseAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                u = u.Substring(_restQueryConfig.BaseAddress.Length);
            }

            // It should start with controller name, but if it wrongly starts with / that / is removed
            if (u.StartsWith("/") && u.Length > 1)
                u = u.Substring(1);

            string name = u;
            int index = u.First(x => x == '/' || x == '?' || x == '$');
            if (index > 0)
            {
                name = u.Substring(0, index);
            }

            foreach (var item in _controllers)
            {
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && item.SupportedType == type)
                {
                    controller = (IRestQueryController)Activator.CreateInstance(item.ControllerType);
                    controller.Context = GetControllerContext(item);
                    break;
                }
            }

            return controller;
        }


        /// <summary>
        /// Initiate the controller list. Use reflection to capture controllers and add them to the dictionary.
        /// A Controller must have the class attribute RestQueryController.
        /// </summary>
        private void InitControllers()
        {
            Assembly[] assarr = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly ass in assarr)
            {
                try
                {
                    Type[] typearr = ass.GetTypes();
                    foreach (Type type in typearr)
                    {
                        if (type.IsClass)
                        {
                            IEnumerable<Attribute> attrArr = type.GetCustomAttributes();
                            if (attrArr != null && attrArr.Count() > 0)
                            {
                                foreach (Attribute attr in attrArr)
                                {
                                    if (attr is RestQueryControllerAttribute)
                                    {
                                        string name = ((RestQueryControllerAttribute)attr).Name;
                                        Type supptype = ((RestQueryControllerAttribute)attr).SupportedType;
                                        Controller controller = new Controller()
                                        {
                                            Name = name,
                                            ControllerType = type,
                                            SupportedType = supptype
                                        };
                                        _controllers.Add(controller);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In some situation an exception is raised which is not harmfull.
                    LogException(ex, false);
                }
            }
        }


        private void LogException(Exception ex, bool fatal = true)
        {
            if (_log != null)
            {
                string err = string.Format("{0} in {1}. Stack Trace. {2}. {3}",
                    ex.Message,
                    ex.Source,
                    ex.StackTrace,
                    ex.InnerException != null ? "Inner Exception: " + ex.InnerException.Message : "");
                if (fatal)
                    _log.Fatal(err);
                else
                    _log.Error(err);
            }
        }


        #endregion
    }

}
