using Microsoft.Ajax.Utilities;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Ultility.Filters
{
    public class EFModelFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema?.properties == null || type == null)
                return;
            ICollection<string> excludedProps = type.GetProperties().Where(w => w.GetGetMethod().IsVirtual).Select(se => se.Name).ToList();
            excludedProps.ForEach(f =>
            {
                if (schema.properties.ContainsKey(f))
                    schema.properties.Remove(f);
            });
        }
    }
}