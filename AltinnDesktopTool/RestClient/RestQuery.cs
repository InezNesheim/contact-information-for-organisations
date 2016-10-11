using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace RestClient
{

    public class RestQuery : IRestQuery
    {
        private IRestQueryConfig _restQueryConfig;
        private ILog _log;

        public RestQuery(IRestQueryConfig restQueryConfig, ILog log)
        {
            _restQueryConfig = restQueryConfig;
            _log = log;
        }

        public T Get<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IList<T> Get<T>(KeyValuePair<string, string> filter)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetByLink<T>(string url)
        {
            throw new NotImplementedException();
        }
    }

}
