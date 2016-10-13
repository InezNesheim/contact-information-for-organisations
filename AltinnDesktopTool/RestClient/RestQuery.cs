using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using RestClient.Controllers;
using RestClient.DTO;
using System.Reflection;
using RestClient.Resources;

namespace RestClient
{

    /// <summary>
    /// The Rest Query class implements IRestQuery. 
    /// </summary>
    /// <remarks>
    /// It is a generic implementation passing queries to IRestQueryControllers to perform query by interpreting 
    /// the url and pass this to the server using the AltinnRestClient.
    /// The Controllers are identified by the  [RestQueryController] attribute and must implement IRestQueryController.
    /// 
    /// Exception Handling:
    /// RestQuery will catch any Exception from Controller, log them and rethrow.
    /// All Exceptions thrown by RestQuery are logged, meaning the caller does not need to log exceptions from RestQuery.
    /// The Controller may log exceptions and errors with caution as RestQuery will log any Exception thrown by the Controller.
    /// </remarks>
    public class RestQuery : IRestQuery
    {
        #region private declarations
        private const string _authenticateUri = "organizations?ForceEIAuthentication";
        private const string CONTROLLER_EXCEPTION_TEXT = "The controller threw an Exception";
        private const string CONTROLLER_NOT_FOUND_FOR_TYPE_EXCEPTION = "No Controller for type {0}";
        private const string CONTROLLER_NOT_FOUND_FOR_URL = "No Controller for url {0}";
        private IRestQueryConfig _restQueryConfig;
        private ILog _log;
        private AltinnRestClient _restClient;
        private bool _isAuthenticated = false;
        private List<RestQueryControllerAttribute> _controllers = new List<RestQueryControllerAttribute>();
        #endregion
        


        #region public properties
        /// <summary>
        /// Gets or sets the configuration as required by the RestQuery.
        /// </summary>
        /// <remarks>
        /// The configuration may be changed in which it will reconnect according to new configuration at first request.
        /// </remarks>
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
        #endregion



        #region constructors
        /// <summary>
        /// Constructs the RestQuery by injecting the configuration and log.
        /// </summary>
        /// <param name="restQueryConfig">The configuration needed for connecting</param>
        /// <param name="log">Optional log4net log instance</param>
        /// <remarks>
        /// The configuration is mandatory, whereas the log is not not mandatory.
        /// </remarks>
        public RestQuery(IRestQueryConfig restQueryConfig, ILog log = null)
        {
            _restQueryConfig = restQueryConfig;
            _log = log;
            _restClient = new AltinnRestClient(restQueryConfig.BaseAddress, restQueryConfig.ApiKey, restQueryConfig.ThumbPrint);
            if (restQueryConfig.Timeout > 0)
                _restClient.Timeout = restQueryConfig.Timeout;
            InitControllers();
        }
        #endregion




        #region IRestQuery implementation
        /// <summary>
        /// Fetches an object by providing and id.
        /// </summary>
        /// <exception cref="RestClientException">
        /// Any Exception from the controller is logged and rethrown as RestClientException with InnerException being the caught Exception.
        /// A RestClientException is also thrown when the controller could not be found supporting type T.
        /// </exception>
        public T Get<T>(string id) where T : HalJsonResource
        {
            EnsureAuthenticated();
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
            {
                string err = string.Format(CONTROLLER_NOT_FOUND_FOR_TYPE_EXCEPTION, typeof(T));
                Log(err, true);
                throw new RestClientException(err);
            }
            try
            {
                return controller.Get<T>(id);
            }
            catch (Exception ex)
            {
                Log(CONTROLLER_EXCEPTION_TEXT, true, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(CONTROLLER_EXCEPTION_TEXT, ex);
            }
        }


        /// <summary>
        /// Search for a list of objects by filtering on a given name value pair.
        /// </summary>
        /// <exception cref="RestClientException">
        /// Any Exception from the controller is logged and rethrown as RestClientException with InnerException being the caught Exception.
        /// A RestClientException is also thrown when the controller could not be found supporting type T.
        /// </exception>
        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            EnsureAuthenticated();
            IRestQueryController controller = GetControllerByType(typeof(T));
            if (controller == null)
            {
                string err = string.Format(CONTROLLER_NOT_FOUND_FOR_TYPE_EXCEPTION, typeof(T));
                Log(err, true);
                throw new RestClientException(err);
            }
            try
            {
                return controller.Get<T>(filter);
            }
            catch (Exception ex)
            {
                Log(CONTROLLER_EXCEPTION_TEXT, true, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(CONTROLLER_EXCEPTION_TEXT, ex);
            }
        }


        /// <summary>
        /// Fetches a list of objects by a given link (url).
        /// </summary>
        /// <exception cref="RestClientException">
        /// Any Exception from the controller is logged and rethrown as RestClientException with InnerException being the caught Exception.
        /// A RestClientException is also thrown when the controller could not be found supporting type T.
        /// </exception>
        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            EnsureAuthenticated();
            IRestQueryController controller = GetControllerByUrl(url);
            if (controller == null)
            {
                string err = string.Format(CONTROLLER_NOT_FOUND_FOR_URL, url);
                Log(err, true);
                throw new RestClientException(err);
            }
            try
            {
                return controller.GetByLink<T>(url);
            }
            catch (Exception ex)
            {
                Log(CONTROLLER_EXCEPTION_TEXT, true, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(CONTROLLER_EXCEPTION_TEXT, ex);
            }
        }
        #endregion



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

        /// <summary>
        /// Generates a controller context to be passed to the controller
        /// </summary>
        /// <param name="attr">The controller's class attribute</param>
        /// <returns>The Controller Contect to be passed to the controller</returns>
        private ControllerContext GetControllerContext(RestQueryControllerAttribute attr)
        {
            return new ControllerContext()
            {
                Log = this._log,
                RestClient = this._restClient,
                ControllerBaseAddress = string.Format("{0}/{1}", _restQueryConfig.BaseAddress, attr.Name)
            };
        }

        /// <summary>
        /// Fetches the controller which supports Type t, meaning the controller which is registered with [RestQueryController(SupportedType = t)]
        /// </summary>
        /// <param name="t">The type matching SupportedType</param>
        /// <returns>The found query controller or null if not found</returns>
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
        private IRestQueryController GetControllerByUrl(string url)
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
            int index = u.IndexOfAny(new char[] { '/', '?', '$' });
            if (index > 0)
            {
                name = u.Substring(0, index);
            }

            foreach (var item in _controllers)
            {
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
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

                                        ((RestQueryControllerAttribute)attr).ControllerType = type;
                                        _controllers.Add((RestQueryControllerAttribute)attr);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In some situation an exception is raised which is not harmfull.
                    Log("Error while browsing assemblies for controllers (harmless)", false, ex);
                }
            }
        }


        /// <summary>
        /// Local logging which takes into account whether _log object is defined or not
        /// </summary>
        /// <param name="text">Error text</param>
        /// <param name="fatal">True if logging is fatal</param>
        /// <param name="ex">Optional exception</param>
        private void Log(string text, bool fatal = true, Exception ex = null)
        {
            if (_log != null)
            {
                try
                {
                    if (fatal)
                        _log.Fatal(text, ex);
                    else
                        _log.Error(text, ex);
                }
                catch { }
            }
        }

        #endregion
    }

}
