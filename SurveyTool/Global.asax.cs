using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NSwag.AspNet.Owin;

namespace SurveyTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //RouteTable.Routes.MapOwinPath("swagger", app =>
            //{
            //    app.UseSwaggerUi3(typeof(MvcApplication).Assembly, settings =>
            //    {
            //        settings.MiddlewareBasePath = "/swagger";
            //        //settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{id}";  //this is the default one
            //        settings.GeneratorSettings.DefaultUrlTemplate = "{controller}/{action}/{id}";
            //    });
            //});
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<Models.ApplicationDbContext>(null);
        }
    }
}
