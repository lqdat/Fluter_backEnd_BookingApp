using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Ultility.Filters
{
    public class SwaggerAuthFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var actionAttributes = apiDescription.GetControllerAndActionAttributes<AuthorizeAttribute>();

            if (!actionAttributes.Any())
            {
                return;
            }

            if (!operation.responses.ContainsKey("401"))
            {
                operation.responses.Add("401", new Response { description = "Không được phép truy cập" });
            }

            if (!operation.responses.ContainsKey("403"))
            {
                operation.responses.Add("403", new Response { description = "Không có quyền truy cập chức năng này" });
            }
            // Thêm security
            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();
            operation.security.Add(new Dictionary<string, IEnumerable<string>>
            {
                { "Bearer", Array.Empty<string>() }
            });
        }
    }

    internal static class SwaggerExtension
    {
        internal static IEnumerable<T> GetControllerAndActionAttributes<T>(this ApiDescription apiDescription) where T : Attribute
        {
            return apiDescription.ActionDescriptor.GetCustomAttributes<T>().Concat(
                apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<T>());
        }
    }
}