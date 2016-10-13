using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestClient.DTO;

namespace RestClient.Deserialize
{
    public class Deserializer
    {
        /// <summary>
        /// Deserializes a list of Typed objects from HAL+JSON format.
        /// Type T should have HalJsonResource as base class
        /// </summary>
        /// <param name="json">Input string to deserialize</param>
        /// <returns></returns>
        public static List<T> DeserializeHalJsonResourceList<T>(string json) where T : HalJsonResource
        {
            var resources = new List<T>();

            try
            {
                var outerResource = JsonConvert.DeserializeObject<OuterJson>(json);
                JObject innerObjectJson = outerResource._embedded;

                var attribute = (PluralNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(PluralNameAttribute));

                JToken resource = innerObjectJson[attribute.PluralName.ToLower()];
                resources = JsonConvert.DeserializeObject<List<T>>(resource.ToString(), new JsonConverter[] { new HalJsonConverter() });
            }
            catch (Exception e)
            {
                //TODO:: Log error
                throw;
            }

            return resources;
        }

        /// <summary>
        /// Deserializes a Typed object from HAL+JSON format
        /// Type T should have HalJsonResource as base class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeHalJsonResource<T>(string json) where T : HalJsonResource
        {
            T resource = null;

            try
            { 
                resource = JsonConvert.DeserializeObject<T>(json, new JsonConverter[] { new HalJsonConverter() });
            }
            catch (Exception e)
            {
                //TODO:: Log error
                throw;
            }

            return resource;
        }
    }
}
