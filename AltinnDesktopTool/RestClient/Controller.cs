using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{
    internal class Controller
    {
        public string Name { get; set; }
        public Type ControllerType { get; set; }
        public Type SupportedType { get; set; }
    }
}
