﻿using RestClient.Deserialize;
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
            var result = Context.RestClient.Get(string.Format("{0}/{1}", Context.ControllerBaseAddress, id));
            if (result != null)
                return Deserializer.DeserializeHalJsonResource<T>(result);
            else
                return null;
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(string.Format("{0}?{1}={2}", Context.ControllerBaseAddress, filter.Key, filter.Value));
            if (result != null)
                return Deserializer.DeserializeHalJsonResourceList<T>(result);
            else
                return null;
        }

        public IList<T> GetByLink<T>(string url) where T : HalJsonResource
        {
            var result = Context.RestClient.Get(url);
            if (result != null)
                return Deserializer.DeserializeHalJsonResourceList<T>(result);
            else
                return null;
        }
    }
}
