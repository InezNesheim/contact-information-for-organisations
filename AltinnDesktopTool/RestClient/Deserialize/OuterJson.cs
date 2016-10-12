using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.Deserialize
{
    /// <summary>
    /// Deserialization wrapper for the outer JSON object which comes with the HAL format
    /// </summary>
    public class OuterJson
    {
        /// <summary>
        ///  Gets or sets the _links container
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public JObject _links { get; set; }

        /// <summary>
        ///  Gets or sets the _embedded container
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public JObject _embedded { get; set; }
    }
}
