using Microsoft.Data.OData;
using Microsoft.Data.Edm;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using BookingApp.WebAPI;

namespace BookingApp.WebAPI.Ultility.Swagger
{
    public class SwaggerODataDocFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            if (swaggerDoc.info.version == "odata")
            {
                IEdmModel edmModel = GlobalConfiguration.Configuration.GetEdmModelV3();
                var converter = new ODataSwaggerConverter(edmModel)
                {
                    Host = swaggerDoc.host,
#if !DEBUG
                    BasePath = "/api/odata",
#endif
                };
                var odataDoc = converter.GetSwaggerModel();
                odataDoc.SelectToken("info.version").Replace(null);
                JsonConvert.PopulateObject(odataDoc.ToString(), swaggerDoc, new JsonSerializerSettings()
                {
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                });
                if (swaggerDoc.security == null)
                    swaggerDoc.security = new List<IDictionary<string, IEnumerable<string>>>();
                foreach (var f in swaggerDoc.securityDefinitions)
                {
                    swaggerDoc.security.Add(new Dictionary<string, IEnumerable<string>>{
                        { f.Key, Array.Empty<string>() }
                    });
                }
            }
        }
    }
}