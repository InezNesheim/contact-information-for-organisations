using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient
{

    /// <summary>
    /// Each class implementing IRestQueryController must have this attribute to identify it as a RestQueryController and define its name.
    /// The name will be used as part of the URL to identify the controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RestQueryControllerAttribute : Attribute
    {
        /// <summary>
        /// Name identifying the controller
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This defines the supported type for this controller.
        /// </summary>
        public Type SupportedType { get; set; }

        public RestQueryControllerAttribute(string name)
        {
            Name = name;
        }
    }
}

