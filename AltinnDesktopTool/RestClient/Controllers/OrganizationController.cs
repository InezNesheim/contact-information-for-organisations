using RestClient.DTO;
using RestClient.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.Controllers
{
    public class OrganizationController : IRestQueryController
    {
        public ControllerContext Context { get; set; }
       

        public T Get<T>(string id) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(string.Format("{0}/{1}", Context.RestClient.BaseAddress, id));
            return Deserializer.DeserializeHalJsonResource<T>(result);            
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(string.Format("{0}?{1}={2}", Context.RestClient.BaseAddress, filter.Key, filter.Value));
            return Deserializer.DeserializeHalJsonResourceList<T>(result);
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(url);
            return Deserializer.DeserializeHalJsonResourceList<T>(result);            
        }
    }
}
