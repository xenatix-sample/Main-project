using System;
using System.ComponentModel;

namespace Axis.Data.Repository.Schema
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SchemaNameAttribute : Attribute
    {
        public string SchemaName { get; private set; }

        public SchemaNameAttribute(string schemaName)
        {
            SchemaName = schemaName;
        }
    }

    public static class SchemaNameAttributeExtensions
    {
        public static string GetSchemaName(this SchemaName schemaName)
        {
            var schemaNameEnum = schemaName.GetType().GetField(schemaName.ToString());
            var customAttributes = schemaNameEnum.GetCustomAttributes(typeof(SchemaNameAttribute), false);
            return customAttributes.Length == 1 ? ((SchemaNameAttribute)customAttributes[0]).SchemaName : string.Empty;
        }
    }
}
