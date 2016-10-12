using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.Controllers
{
    public class OrganizationController : IRestQueryController
    {
        public ControllerContext Context
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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
