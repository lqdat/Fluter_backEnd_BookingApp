using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Ultility.Filters
{
    public class SecurityFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            IEnumerable<string> scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
                .Select(filterInfo => filterInfo.Instance)
                .OfType<AuthorizeAttribute>()
                .SelectMany(attr => attr.Roles.Split(','))
                .Distinct();
            if (scopes.Any())
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();
                operation.parameters.Add(new Parameter
                {
                    name = "Bearer",
                    @in = "header",
                    type = "string",
                    required = true,
                    description = "Bearer token"
                });
            }
        }
    }
}