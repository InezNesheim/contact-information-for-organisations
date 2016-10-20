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
            var result = this.Context.RestClient.Get($"{this.Context.ControllerBaseAddress}/{id}");
            return result != null ? Deserializer.DeserializeHalJsonResource<T>(result) : null;
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            var result = this.Context.RestClient.Get(
                $"{this.Context.ControllerBaseAddress}?{filter.Key}={filter.Value}");
            return result != null ? Deserializer.DeserializeHalJsonResourceList<T>(result) : null;
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            var result = this.Context.RestClient.Get(url);
            return result != null ? Deserializer.DeserializeHalJsonResourceList<T>(result) : null;
        }
    }
}
