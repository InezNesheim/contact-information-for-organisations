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
    /// HAL and JSON converter class for the ResourceList type
    /// </summary>
    public class ResourceListConverter : JsonConverter
    {
        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// is ResourceList
        /// </summary>
        /// <param name="objectType">Type to be checked</param>
        /// <returns>Is ResourceList</returns>
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
            var list = (IResourceList)value;

            list.Links.Add(new Link
            {
                Rel = "self",
                Href = list.Href
            });

            writer.WriteStartObject();
            writer.WritePropertyName("_links");
            serializer.Serialize(writer, list.Links);

            writer.WritePropertyName("_embedded");
            writer.WriteStartObject();
            writer.WritePropertyName(list.Rel);
            writer.WriteStartArray();

            foreach (Resource halResource in list)
            {
                serializer.Serialize(writer, halResource);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();

            var listType = list.GetType();
            var propertyInfos = typeof(ResourceList<>).GetProperties().Select(p => p.Name);
            foreach (var property in listType.GetProperties().Where(p => !propertyInfos.Contains(p.Name)))
            {
                writer.WritePropertyName(property.Name.ToLower());
                serializer.Serialize(writer, property.GetValue(value, null));
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads JSON
        /// </summary>
        /// <param name="reader">The JSON reader</param>
        /// <param name="objectType">The type</param>
        /// <param name="existingValue">The existing value</param>
        /// <param name="serializer">The JSON serializer</param>
        /// <returns>The read object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (typeof(Forms).IsAssignableFrom(objectType))
            {
                List<Form> forms = new List<Form>();
                JArray jsonObject = JArray.Load(reader);
                serializer.Populate(jsonObject.CreateReader(), forms);

                return new Forms(forms);
            }
            else if (typeof(Attachments).IsAssignableFrom(objectType))
            {
                List<Attachment> attachments = new List<Attachment>();
                JArray jsonObject = JArray.Load(reader);
                serializer.Populate(jsonObject.CreateReader(), attachments);

                return new Attachments(attachments);
            }
            else if (typeof(Roles).IsAssignableFrom(objectType))
            {
                List<Role> roles = new List<Role>();
                JArray jsonObject = JArray.Load(reader);
                serializer.Populate(jsonObject.CreateReader(), roles);

                return new Roles(roles);
            }
            else if (typeof(Rights).IsAssignableFrom(objectType))
            {
                List<Right> roles = new List<Right>();
                JArray jsonObject = JArray.Load(reader);
                serializer.Populate(jsonObject.CreateReader(), roles);

                return new Rights(roles);
            }

            return reader.Value;
        }

        /// <summary>
        /// Checks if the given <paramref name="objectType"/>
        /// can be converted to Resource and is not ResourceList
        /// </summary>
        /// <param name="objectType">The type to be checked</param>
        /// <returns>Can be converted to Resource</returns>
        public override bool CanConvert(Type objectType)
        {
            return IsResource(objectType) && IsResourceList(objectType);
        }
    }
}