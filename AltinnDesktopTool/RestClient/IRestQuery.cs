using System.Collections.Generic;
using RestClient.DTO;

namespace RestClient
{
    public interface IRestQuery
    {
        IList<T> GetByLink<T>(string url) where T : HalJsonResource;

        IList<T>Get<T>(KeyValuePair<string, string> filter) where T : HalJsonResource;

        T Get<T>(string id) where T : HalJsonResource;
    }
}
