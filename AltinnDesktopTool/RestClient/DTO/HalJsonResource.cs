using System;

namespace RestClient.DTO
{
    /// <summary>
    /// Base class for Dto objects - deserializable from HAL+JSON format
    /// </summary>
    public class HalJsonResource
    {
        public HalJsonResource()
        {
            var t = this.GetType();
            if (t.IsDefined(typeof(PluralNameAttribute), false) == false)
            {
                throw new InvalidOperationException("Missing Plural name attribute on HalJsonResource class!");
            }
        }
    }

    /// <summary>
    /// Attribute for indicating object list name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : Attribute
    {
        public readonly string PluralName;

        public PluralNameAttribute(string pluralName)
        {
            this.PluralName = pluralName;
        }
    }
}
