using Microsoft.Owin;
using Owin;
using System.Web.Http;
using NSwag.AspNet.Owin;

[assembly: OwinStartupAttribute(typeof(SurveyTool.Startup))]
namespace SurveyTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var config = new HttpConfiguration();
            ////config.EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API")).EnableSwaggerUi();
            ////app.Use(config);

            ////app.UseSwaggerUi3(typeof(Startup).GetType().Assembly, settings =>
            ////{
            ////    settings.GeneratorSettings.DefaultUrlTemplate = "{controller}/{action}/{id?}";
            ////});
            //app.UseWebApi(config);
            ConfigureAuth(app);
        }
    }
}
