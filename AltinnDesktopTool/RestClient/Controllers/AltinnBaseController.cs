using RestClient.DTO;
using RestClient.Util;
using System.Collections.Generic;

namespace RestClient.Controllers
{
    [RestQueryController(Name = "organizations", SupportedType = typeof(Organization))]
    public class AltinnBaseController : IRestQueryController
    {
        public ControllerContext Context { get; set; }


        public T Get<T>(string id) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(string.Format("{0}/{1}", Context.ControllerBaseAddress, id));
            return Deserializer.DeserializeHalJsonResource<T>(result);            
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(string.Format("{0}?{1}={2}", Context.ControllerBaseAddress, filter.Key, filter.Value));
            return Deserializer.DeserializeHalJsonResourceList<T>(result);
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(url);
            return Deserializer.DeserializeHalJsonResourceList<T>(result);            
        }
    }
}
