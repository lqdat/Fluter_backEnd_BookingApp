using Swashbuckle.Swagger;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Description;

namespace CSDLGiaBinhDinh.WebAPI.Ultility.Swagger
{
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerFileUploadAttribute>().SingleOrDefault()
                is SwaggerFileUploadAttribute attribute)
            {
                operation.consumes.Add("multipart/form-data");
                var modelType = attribute.ModelType;
                var props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(w => !w.GetGetMethod().IsVirtual);
                operation.parameters = props.Select(se => {
                    var param = new Parameter()
                    {
                        @in = "formData",
                        name = se.GetCustomAttribute<DisplayAttribute>()?.Name ?? se.Name,
                        description = se.GetCustomAttribute<DescriptionAttribute>()?.Description,
                        required = se.GetCustomAttribute<RequiredAttribute>() != null
                            || (se.PropertyType != typeof(string) && Nullable.GetUnderlyingType(se.PropertyType) == null)
                    };
                    if (se.PropertyType == typeof(HttpPostedFile))
                    {
                        param.type = "file";
                    }
                    else
                    {
                        var schema = schemaRegistry.GetOrRegister(se.PropertyType);
                        param.PopulateFrom(schema);
                    }

                    return param;
                }).ToArray();
            }
        }
    }
}
