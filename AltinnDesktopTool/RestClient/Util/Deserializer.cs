using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestClient.DTO;

namespace RestClient.Util
{
    public class Deserializer
    {
        /// <summary>
        /// De-serialize the list of organizations from JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Organization> DeserializeOrganizations(string json)
        {
            var orgs = new List<Organization>();
            try
            {
                var outerOrg = JsonConvert.DeserializeObject<OuterJson>(json);
                JObject innerObjectJsonOrg = outerOrg._embedded;
                JToken organization = innerObjectJsonOrg["organizations"];
                orgs = JsonConvert.DeserializeObject<List<Organization>>(organization.ToString(), new JsonConverter[] { new OrganizationConverter() });
                //orgs = organization.ToObject<IList<Organization>>().ToList();
            }
            catch (Exception e)
            {
                //TODO:: log error
                //Logger.Logg("Failed to deserialize Json: ", e);
            }

            return orgs;
        }

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

        public class OrganizationConverter : JsonConverter
        {
            /// <summary>
            /// Checks if the given <paramref name="objectType"/>
            /// is Organization
            /// </summary>
            /// <param name="objectType">Object type</param>
            /// <returns>True if its an organization else false</returns>
            public static bool IsOrganization(Type objectType)
            {
                return typeof(Organization).IsAssignableFrom(objectType);
            }

            /// <summary>
            /// Writes the given <paramref name="value"/> as JSON
            /// </summary>
            /// <param name="writer">The JSON writer</param>
            /// <param name="value">Value to be written</param>
            /// <param name="serializer">The JSON serializer</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Reads organization from JSON
            /// </summary>
            /// <param name="reader">The JSON reader</param>
            /// <param name="objectType">The type (Organization)</param>
            /// <param name="existingValue">The existing value</param>
            /// <param name="serializer">The JSON serializer</param>
            /// <returns>The read object</returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                            JsonSerializer serializer)
            {
                var obj = JToken.ReadFrom(reader);
                var ret = JsonConvert.DeserializeObject(obj.ToString(), objectType, new JsonConverter[] { });

                // Deserialize any embedded resources (typically ResourceLists)
                if (obj["_links"] != null && obj["_links"].HasValues)
                {
                    var enumeratorEmbedded = ((JObject)obj["_links"]).GetEnumerator();
                    while (enumeratorEmbedded.MoveNext())
                    {
                        string rel = enumeratorEmbedded.Current.Key;

                        foreach (var property in objectType.GetProperties())
                        {
                            bool attribute = property.Name.ToLower() == rel.ToLower();

                            if (attribute)
                            {
                                property.SetValue(ret, obj["_links"][rel]["href"].ToString());
                            }
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Checks if the given <paramref name="objectType"/>
            /// can be converted to Organization
            /// </summary>
            /// <param name="objectType">Type to be checked</param>
            /// <returns>Can be converted to Organization</returns>
            public override bool CanConvert(Type objectType)
            {
                return IsOrganization(objectType);
            }
        }
    }
}
