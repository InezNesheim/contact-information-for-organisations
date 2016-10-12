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
        private AltinnRestClient _restClient;

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

        public RestQuery(IRestQueryConfig restQueryConfig, ILog log)
        {
            _restQueryConfig = restQueryConfig;
            _log = log;
            _restClient = new AltinnRestClient(restQueryConfig.BaseAddress, restQueryConfig.ApiKey, restQueryConfig.ThumbPrint);
            if (restQueryConfig.Timeout > 0)
                _restClient.Timeout = restQueryConfig.Timeout;
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
