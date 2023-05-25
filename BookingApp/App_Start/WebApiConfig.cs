using BookingApp.App_Start;
using BookingApp.DataProvider.EF;
using BookingApp.WebAPI;
using Newtonsoft.Json;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

namespace BookingApp.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore };
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services
            
            config.Routes.MapODataServiceRoute("odata", "odata", config.GetEdmModelV3());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("SwaggerUI", "", null, null, new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
            config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            // Config Dependency Resolver
            config.DependencyResolver = new NinjectResolver();

        }
    }
}
