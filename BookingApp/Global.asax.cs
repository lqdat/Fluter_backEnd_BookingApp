using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using BookingApp.WebAPI;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace BookingApp
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            /*AreaRegistration.RegisterAllAreas();*/
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(SwaggerConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(HttpContext.Current.Server.MapPath("~/service-account-file.json")),
            });
            /*  HttpConfiguration config = GlobalConfiguration.Configuration;
              config.Formatters.JsonFormatter
                          .SerializerSettings
                          .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;*/
        }
    }
}
