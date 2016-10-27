using System;
using RestClient.Deserialize;
using RestClient.DTO;
using System.Collections.Generic;

namespace RestClient.Controllers
{
    /// <summary>
    /// Generic Controller, currently supporting organizations. Extendible by adding additional attributes as long as the same URL pattern is used.
    /// </summary>
    [RestQueryController(Name = "organizations", SupportedType = typeof(Organization))]
    public class AltinnBaseController : IRestQueryController
    {
        public ControllerContext Context { get; set; }


        public T Get<T>(string id) where T : HalJsonResource
        {
            string url = $"{this.Context.ControllerBaseAddress}/{id}?ForceEIAuthentication";
            string result = this.Context.RestClient.Get(url);
            return result != null ? Deserializer.DeserializeHalJsonResource<T>(result) : null;
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            string url = $"{this.Context.ControllerBaseAddress}?ForceEIAuthentication&{filter.Key}={filter.Value}";
            string result = this.Context.RestClient.Get(url);
            return result != null ? Deserializer.DeserializeHalJsonResourceList<T>(result) : null;
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            url += url.IndexOf("?",StringComparison.InvariantCulture) > 0 ? "&" : "?";
            url += "ForceEIAuthentication";
            string result = this.Context.RestClient.Get(url);
            return result != null ? Deserializer.DeserializeHalJsonResourceList<T>(result) : null;
        }
    }
}
