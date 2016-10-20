﻿using System;
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
        private const string AuthenticateUri = "organizations?ForceEIAuthentication";
        private const string ControllerExceptionText = "The controller threw an Exception";
        private const string ControllerNotFoundForTypeException = "No Controller for type {0}";
        private const string ControllerNotFoundForUrl = "No Controller for url {0}";
        private IRestQueryConfig restQueryConfig;
        private readonly ILog log;
        private readonly AltinnRestClient restClient;
        private bool isAuthenticated = false;
        private readonly List<RestQueryControllerAttribute> controllers = new List<RestQueryControllerAttribute>();
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
                return this.restQueryConfig;
            }
            set
            {
                this.restQueryConfig = value;
                this.restClient.BaseAddress = this.restQueryConfig.BaseAddress;
                this.restClient.ApiKey = this.restQueryConfig.ApiKey;
                this.restClient.Thumbprint = this.restQueryConfig.ThumbPrint;
                if (this.restQueryConfig.Timeout > 0)
                    this.restClient.Timeout = this.restQueryConfig.Timeout;
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
            this.restQueryConfig = restQueryConfig;
            this.log = log;
            this.restClient = new AltinnRestClient(restQueryConfig.BaseAddress, restQueryConfig.ApiKey, restQueryConfig.ThumbPrint);
            if (restQueryConfig.Timeout > 0)
                this.restClient.Timeout = restQueryConfig.Timeout;
            this.InitControllers();
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
            this.EnsureAuthenticated();
            var controller = this.GetControllerByType(typeof(T));
            if (controller == null)
            {
                var err = string.Format(ControllerNotFoundForTypeException, typeof(T));
                this.Log(err, LogLevel.Error);
                throw new RestClientException(err);
            }
            try
            {
                return controller.Get<T>(id);
            }
            catch (Exception ex)
            {
                this.Log(ControllerExceptionText, LogLevel.Error, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(ControllerExceptionText, ex);
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
            this.EnsureAuthenticated();
            var controller = this.GetControllerByType(typeof(T));
            if (controller == null)
            {
                var err = string.Format(ControllerNotFoundForTypeException, typeof(T));
                this.Log(err, LogLevel.Error);
                throw new RestClientException(err);
            }
            try
            {
                return controller.Get<T>(filter);
            }
            catch (Exception ex)
            {
                this.Log(ControllerExceptionText, LogLevel.Error, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(ControllerExceptionText, ex);
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
            this.EnsureAuthenticated();
            var controller = this.GetControllerByUrl(url);
            if (controller == null)
            {
                var err = string.Format(ControllerNotFoundForUrl, url);
                this.Log(err, LogLevel.Error);
                throw new RestClientException(err);
            }
            try
            {
                return controller.GetByLink<T>(url);
            }
            catch (Exception ex)
            {
                this.Log(ControllerExceptionText, LogLevel.Error, ex);
                if (ex is RestClientException)
                    throw;
                else
                    throw new RestClientException(ControllerExceptionText, ex);
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
            if (!this.isAuthenticated)
            {
                try
                {
                    this.restClient.Get(AuthenticateUri);
                }
                catch
                {
                }
                this.isAuthenticated = true;
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
                Log = this.log,
                RestClient = this.restClient,
                ControllerBaseAddress = $"{this.restQueryConfig.BaseAddress}/{attr.Name}"
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
            foreach (var item in this.controllers)
            {
                if (item.SupportedType == t)
                {
                    controller = (IRestQueryController)Activator.CreateInstance(item.ControllerType);
                    controller.Context = this.GetControllerContext(item);
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
            var u = url;
            if (u.StartsWith(this.restQueryConfig.BaseAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                u = u.Substring(this.restQueryConfig.BaseAddress.Length);
            }

            // It should start with controller name, but if it wrongly starts with / that / is removed
            if (u.StartsWith("/") && u.Length > 1)
                u = u.Substring(1);

            var name = u;
            var index = u.IndexOfAny(new char[] { '/', '?', '$' });
            if (index > 0)
            {
                name = u.Substring(0, index);
            }

            foreach (var item in this.controllers)
            {
                if (!item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) continue;
                controller = (IRestQueryController)Activator.CreateInstance(item.ControllerType);
                controller.Context = this.GetControllerContext(item);
                break;
            }

            return controller;
        }


        /// <summary>
        /// Initiate the controller list. Use reflection to capture controllers and add them to the dictionary.
        /// A Controller must have the class attribute RestQueryController.
        /// </summary>
        private void InitControllers()
        {
            var assarr = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in assarr)
            {
                try
                {
                    var typearr = ass.GetTypes();
                    foreach (var type in typearr)
                    {
                        if (!type.IsClass) continue;
                        var attrArr = type.GetCustomAttributes();
                        if (attrArr == null) continue;
                        foreach (var attr in attrArr)
                        {
                            var item = attr as RestQueryControllerAttribute;
                            if (item == null) continue;
                            var name = item.Name;
                            var supptype = item.SupportedType;
                            item.ControllerType = type;
                            this.controllers.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In some situation an exception is raised which is not harmfull.
                    this.Log("Error while browsing assemblies for controllers (harmless)", LogLevel.Warning, ex);
                }
            }
        }


        /// <summary>
        /// Local logging which takes into account whether _log object is defined or not
        /// </summary>
        /// <param name="text">Error text</param>
        /// <param name="level">Logging level</param>
        /// <param name="ex">Optional exception</param>
        private void Log(string text,  LogLevel level, Exception ex = null)
        {
            if (this.log == null) return;
            try
            {
                switch(level)
                {
                    case LogLevel.Debug:
                        this.log.Debug(text);
                        break;
                    case LogLevel.Error:
                        this.log.Error(text, ex);
                        break;
                    case LogLevel.Warning:
                        this.log.Warn(text);
                        break;
                    case LogLevel.Info:
                        this.log.Info(text);
                        break;
                    case LogLevel.Fatal:
                        this.log.Fatal(text, ex);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }                        
            }
            catch { }
        }

        public enum LogLevel
        {
            Debug = 0,
            Error,
            Warning,
            Info,
            Fatal,
        }
        #endregion
    }

}
