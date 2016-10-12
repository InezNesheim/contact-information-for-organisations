using System;

namespace RestClient.DTO
{
    public class HalJsonResource
    {
        public HalJsonResource()
        {
            Type t = GetType();
            if (t.IsDefined(typeof(PluralNameAttribute), false) == false)
            {
                throw new InvalidOperationException("Missing Plural name attribute on HalJsonResource class!");

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : Attribute
    {
        public readonly string PluralName;

        public PluralNameAttribute(string pluralName)
        {
            PluralName = pluralName;
        }
    }
}
