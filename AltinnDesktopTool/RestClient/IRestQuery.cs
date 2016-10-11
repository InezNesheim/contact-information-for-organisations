using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{
    interface IRestQuery
    {
        IList<T>GetByLink<T>(string url);

        IList<T>Get<T>(KeyValuePair<string, string> filter);

        T Get<T>(string id);
    }
}

