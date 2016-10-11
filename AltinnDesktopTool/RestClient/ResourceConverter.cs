using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Altinn.SBL.AltinnWebAPI.Interfaces;
using Altinn.SBL.AltinnWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Altinn.SBL.AltinnWebAPI.JsonConverters
{
    /// <summary>
    /// HAL and JSON converter class for the Resource type
    /// </summary>
    public class ResourceConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConverter"/> class
        /// </summary>
        /// <param name="type">
        /// Type of the object currently converted
        /// </param>
        public ResourceConverter(Type type = null)
        {
            ObjectType = type;
        }

        /// <summary>
        /// Gets or sets the current ObjectType
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// is IResourceList
        /// </summary>
        /// <param name="objectType">Type to be checked</param>
        /// <returns>Is resource list</returns>
        public static bool IsResourceList(Type objectType)
        {
            return typeof(IResourceList).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// is Resource
        /// </summary>
        /// <param name="objectType">Type to be checked</param>
        /// <returns>Is Resource</returns>
        public static bool IsResource(Type objectType)
        {
            return typeof(Resource).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Writes the given <paramref name="value"/>
        /// as JSON
        /// </summary>
        /// <param name="writer">The JSON writer</param>
        /// <param name="value">Value to be written</param>
        /// <param name="serializer">The JSON serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resource = (Resource)value;

            if (!String.IsNullOrEmpty(resource.Href) && !String.IsNullOrEmpty(resource.Rel) && resource.Links != null)
            {
                resource.Links.Insert(0, new Link
                {
                    Rel = "self",
                    Href = resource.Href
                });
            }

            serializer.Converters.Remove(this);
            serializer.Serialize(writer, resource);
            serializer.Converters.Add(this);
        }

        /// <summary>
        /// Reads JSON
        /// </summary>
        /// <param name="reader">The JSON reader</param>
        /// <param name="objectType">The object Type</param>
        /// <param name="existingValue">The existing value</param>
        /// <param name="serializer">The JSON serializer</param>
        /// <returns>The read object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            JToken obj = JToken.ReadFrom(reader);
            var ret = JsonConvert.DeserializeObject(obj.ToString(), ObjectType ?? objectType, new JsonConverter[] { });

            // Deserialize any embedded resources (typically ResourceLists)
            if (obj["_embedded"] != null && obj["_embedded"].HasValues)
            {
                var enumerator = ((JObject)obj["_embedded"]).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string rel = enumerator.Current.Key;
                    foreach (var property in objectType.GetProperties())
                    {
                        bool attribute = property.Name.ToLower() == rel.ToLower();

                        if (attribute)
                        {
                            Type type = property.PropertyType;

                            property.SetValue(ret, 
                                JsonConvert.DeserializeObject(enumerator.Current.Value.ToString(), 
                                type, 
                                new ResourceListConverter()), 
                                null);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// can be converted to Resource and is not a ResourceList
        /// </summary>
        /// <param name="objectType">Type to be checked</param>
        /// <returns>Can be converted to Resource</returns>
        public override bool CanConvert(Type objectType)
        {
            return IsResource(objectType) && !IsResourceList(objectType);
        }
    }
}