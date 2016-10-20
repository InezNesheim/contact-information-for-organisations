﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestClient.DTO;
using RestClient.Resources;

namespace RestClient.Deserialize
{
    public class Deserializer
    {
        private static readonly string ErrorOnDeserialization = "Error while deserializing Json data";

        /// <summary>
        /// Deserializes a list of Typed objects from HAL+JSON format.
        /// Type T should have HalJsonResource as base class
        /// </summary>
        /// <param name="json">Input string to deserialize</param>
        /// <returns></returns>
        public static List<T> DeserializeHalJsonResourceList<T>(string json) where T : HalJsonResource
        {
            List<T> resources;

            try
            {
                var outerResource = JsonConvert.DeserializeObject<OuterJson>(json);
                var innerObjectJson = outerResource._embedded;

                var attribute = (PluralNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(PluralNameAttribute));

                var resource = innerObjectJson[attribute.PluralName.ToLower()];
                resources = JsonConvert.DeserializeObject<List<T>>(resource.ToString(), new HalJsonConverter());
            }
            catch (Exception e)
            {
                throw new RestClientException(ErrorOnDeserialization, e);
                // Note, there is no need for logging here, as logging is done by RestClient.
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
            T resource;

            try
            { 
                resource = JsonConvert.DeserializeObject<T>(json, new HalJsonConverter());
            }
            catch (Exception e)
            {
                throw new RestClientException(ErrorOnDeserialization, e);
                // Note, there is no need for logging here, as logging is done by RestClient.
            }

            return resource;
        }
    }
}
