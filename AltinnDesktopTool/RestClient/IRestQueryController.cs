using RestClient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{
    public interface IRestQueryController : IRestQuery
    {
        ControllerContext Context { get; set; }
    }
}
