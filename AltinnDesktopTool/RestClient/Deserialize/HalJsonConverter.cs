using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestClient.DTO;
using System;

namespace RestClient.Deserialize
{
    public class HalJsonConverter : JsonConverter
    {
        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// is HalJsonResource
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <returns>True if its an organization else false</returns>
        public static bool IsHalJsonResource(Type objectType)
        {
            return typeof(HalJsonResource).IsAssignableFrom(objectType);
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
        /// Reads haljsonResource from JSON
        /// </summary>
        /// <param name="reader">The JSON reader</param>
        /// <param name="objectType">The type (child of HalJsonReader)</param>
        /// <param name="existingValue">The existing value</param>
        /// <param name="serializer">The JSON serializer</param>
        /// <returns>The read object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var obj = JToken.ReadFrom(reader);
            var ret = JsonConvert.DeserializeObject(obj.ToString(), objectType, new JsonConverter[] { });

            //TODO:: deserialize _embedded

            // Deserialize _links
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
        /// can be converted to HalJsonResource
        /// </summary>
        /// <param name="objectType">Type to be checked</param>
        /// <returns>Can be converted to HalJsonResource</returns>
        public override bool CanConvert(Type objectType)
        {
            return IsHalJsonResource(objectType);
        }
    }
}
